using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Dtos.Babies;
using BabyMonitorApiDataAccess.Dtos.BabyState;
using BabyMonitorApiDataAccess.Dtos.Devices;
using BabyMonitorApiDataAccess.Dtos.Livestream;
using BabyMonitorApiDataAccess.Entities;
using BabyMonitorApiDataAccess.Repositories.Abstractions;

namespace BabyMonitorApiBusiness.Concretes
{
    public class LivestreamService : ILivestreamService
    {
        private IRepository<Livestream> _livestreamRepository;
        private IUserRepository _userRepository;
        private IRepository<User> _baseUserRepository;
        private IRepository<Baby> _babyRepository;
        private IRepository<MonitoringDevice> _monitoringDeviceRepository;
        private IRepository<BabyState> _babyStateRepository;

        public LivestreamService(IRepository<Livestream> livestreamRepository, IUserRepository userRepository, IRepository<Baby> babyRepository, IRepository<MonitoringDevice> monitoringDeviceRepository, IRepository<BabyState> babyStateRepository, IRepository<User> baseUserRepository)
        {
            _livestreamRepository = livestreamRepository;
            _userRepository = userRepository;
            _babyRepository = babyRepository;
            _monitoringDeviceRepository = monitoringDeviceRepository;
            _babyStateRepository = babyStateRepository;
            _baseUserRepository = baseUserRepository;
        }

        public async Task<Livestream> CreateLivestream(CreateLivestreamDto createLivestream)
        {
            try
            {
                // This hardcoded check is temporary for testing purposes
                if (createLivestream.ServerKey != "85159608637075381844")
                {
                    throw new ServerKeyNotValidException("Server key not valid");
                }

                // Find monitoring device
                MonitoringDevice? foundDevice = await _monitoringDeviceRepository.GetByIdAsync(createLivestream.DeviceId);
                if (foundDevice == null)
                {
                    throw new MonitoringDeviceNotFoundException("Monitoring device not found");
                }

                // Find user
                User? user = await _baseUserRepository.GetByIdAsync(foundDevice.UserId);
                if (user == null) 
                {
                    throw new UserNotFoundException("User not found");
                }

                // Find baby
                Baby? baby = await _babyRepository.GetByIdAsync(foundDevice.BabyId);
                if (baby == null)
                {
                    throw new BabyNotFoundException("Baby not found");
                }

                Livestream newLivestream = new Livestream()
                {
                    Id = Guid.NewGuid(),
                    BabyId = baby.Id,
                    DateStarted = DateTime.Parse(createLivestream.DateStarted ?? "0-0-0"),
                    Time = createLivestream.Time,
                    DeviceId = createLivestream.DeviceId
                };

                Livestream? createdLivestream = await _livestreamRepository.PostAsync(newLivestream);

                if (createdLivestream == null)
                {
                    throw new LivestreamCreationException("Problem when creating livestream");
                }

                // Create baby states
                if (createLivestream.BabyStates != null && createLivestream.BabyStates.Count > 0)
                {
                    foreach (var babyState in createLivestream.BabyStates)
                    {
                        BabyState newBabyState = new BabyState()
                        {
                            Id = Guid.NewGuid(),
                            AtSecond = babyState.AtSecond,
                            Awake = babyState.Awake,
                            LivestreamId = createdLivestream.Id,
                            Emotion = babyState.Emotion
                        };

                        BabyState createdBabyState = await _babyStateRepository.PostAsync(newBabyState);
                        if (createdBabyState == null)
                        {
                            throw new BabyStateCreationException("Problem when creating baby state");
                        }
                    }

                    return createdLivestream;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task DeleteLivestream(Guid id, string userEmail)
        {
            try
            {
                User? user = await _userRepository.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                Livestream? livestream = await _livestreamRepository.GetByIdAsync(id);
                if (livestream == null) 
                {
                    throw new LivestreamNotFoundException("Livestream not found");
                }

                Baby? baby = await _babyRepository.GetByIdAsync(livestream.BabyId);
                if (baby == null)
                {
                    throw new BabyNotFoundException("Baby not found");
                }

                if (!baby.UserId.Equals(user.Id))
                {
                    throw new LivestreamNotAllowedToAccessException("Livestream not allowed to access");
                }

                await _livestreamRepository.DeleteAsync(id);

                // Delete all baby states associated with livestream
                List<BabyState> babyStates = _babyStateRepository.
                    GetAllAsync().
                    Result.
                    ToList().
                    Where(bs => bs.LivestreamId == livestream.Id).
                    ToList();
                if (babyStates.Count > 0)
                {
                    foreach (var state in babyStates)
                    {
                        Guid stateId = (Guid) state.Id;
                        await _babyStateRepository.DeleteAsync(stateId);
                    }
                }
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (LivestreamNotFoundException)
            {
                throw;
            }
            catch (BabyNotFoundException)
            {
                throw;
            }
            catch (LivestreamNotAllowedToAccessException)
            {
                throw;
            }
        }

        public async Task<List<GetAllLivestreamsDto>> GetAllLivestreams(string userEmail)
        {
            try
            {
                User? user = await _userRepository.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                List<Livestream> livestreams = _livestreamRepository.GetAllAsync().Result.ToList();
                List<GetAllLivestreamsDto> getLivestreams = new List<GetAllLivestreamsDto>();
                foreach (Livestream l in livestreams)
                {
                    Baby? baby = await _babyRepository.GetByIdAsync(l.BabyId);
                    var device = await _monitoringDeviceRepository.GetByIdAsync(l.DeviceId);
                    if (device == null)
                    {
                        throw new MonitoringDeviceNotFoundException("Monitoring device not found");
                    }
                    
                    if (baby != null && baby.UserId.Equals(user.Id))
                    {
                        getLivestreams.Add(new GetAllLivestreamsDto()
                        {
                            Id = l.Id,
                            BabyId = l.BabyId,
                            Time = l.Time,
                            DateStarted = l.DateStarted,
                            Device = new GetLivestreamDeviceDto()
                            {
                                Name = device.Name 
                            },
                            Baby = new CreatedBabyDto()
                            {
                                Id = baby.Id,
                                Birthdate = baby.BirthDate,
                                Name = baby.Name,
                                PhotoUrl = baby.PhotoUrl,
                                UserId = baby.UserId
                            }
                        });
                    }
                }
                return getLivestreams.OrderBy(l => l.DateStarted).ToList();
            }
            catch (UserNotFoundException)
            {
                throw;
            }
        }

        public async Task<GetLivestreamDto> GetLivestream(Guid id, string userEmail)
        {
            try
            {
                User? user = await _userRepository.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                Livestream? livestream = await _livestreamRepository.GetByIdAsync(id);
                if (livestream == null)
                {
                    throw new LivestreamNotFoundException("Livestream not found");
                }

                Baby? baby = await _babyRepository.GetByIdAsync(livestream.BabyId);
                if (baby == null)
                {
                    throw new BabyNotFoundException("Baby not found");
                }

                if (!baby.UserId.Equals(user.Id))
                {
                    throw new LivestreamNotAllowedToAccessException("Livestream not allowed to access");
                }

                GetLivestreamDto getLiveStream = new GetLivestreamDto()
                {
                    Id = livestream.Id,
                    BabyId = livestream.BabyId,
                    DateStarted = livestream.DateStarted,
                    Time = livestream.Time,
                    BabyStates = new List<CreateBabyStateDto>(),
                    Baby = new CreatedBabyDto()
                    {
                        Id = livestream._Baby.Id,
                        Birthdate = livestream._Baby.BirthDate,
                        Name = livestream._Baby.Name,
                        PhotoUrl = livestream._Baby.PhotoUrl,
                        UserId = livestream._Baby.UserId
                    }
                };

                List<BabyState> babyStates = _babyStateRepository.GetAllAsync().Result.ToList();
                if (babyStates != null && babyStates.Count > 0)
                {
                    getLiveStream.BabyStates = babyStates.
                        Where(bs => bs.LivestreamId == livestream.Id).
                        Select(bs => new CreateBabyStateDto()
                        {
                            AtSecond = bs.AtSecond,
                            Awake = bs.Awake,
                            Emotion = bs.Emotion
                        }).ToList();
                }

                return getLiveStream;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (LivestreamNotFoundException)
            {
                throw;
            }
            catch (BabyNotFoundException)
            {
                throw;
            }
            catch (LivestreamNotAllowedToAccessException)
            {
                throw;
            }
        }

        public async Task<GetStreamStatisticsDto> GetLivestreamStatistics(Guid id, string userEmail)
        {
            try
            {
                // Find user
                User? user = await _userRepository.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                // Find livestream
                Livestream? livestream = await _livestreamRepository.GetByIdAsync(id);
                if (livestream == null)
                {
                    throw new LivestreamNotFoundException("Livestream not found");
                }

                // Find baby
                Baby? baby = await _babyRepository.GetByIdAsync(livestream.BabyId);
                if (baby == null)
                {
                    throw new BabyNotFoundException("Baby not found");
                }

                if (!baby.UserId.Equals(user.Id))
                {
                    throw new LivestreamNotAllowedToAccessException("Livestream not allowed to access");
                }

                // Find all the baby states associated to livestream
                List<CreateBabyStateDto> babyStates = _babyStateRepository.
                    GetAllAsync().
                    Result.
                    ToList().
                    Where(bs => bs.LivestreamId == id).
                    OrderBy(bs => bs.AtSecond).
                    ToList().
                    Select(bs => new CreateBabyStateDto()
                    {
                        Awake = bs.Awake,
                        AtSecond = bs.AtSecond,
                        Emotion = bs.Emotion
                    }).
                    ToList();

                if (babyStates.Count > 0)
                {
                    // Calculate total sleep duration
                    int totalSleepDuration = babyStates.
                        Where(bs => bs.Awake == false)
                        .Count();

                    // Calculate frequency of emotions
                    Dictionary<string, int> emotionFrequency = new Dictionary<string, int>();

                    // Separate baby state in sleep intervals
                    List<BabyStateSleepIntervalDto> babyStatesSleepIntervals = new List<BabyStateSleepIntervalDto>();

                    emotionFrequency.Add(babyStates[0].Emotion, 1);

                    bool currentElementAwake = babyStates[0].Awake;
                    var babyStatesLength = babyStates.Count;
                    var countPercentage = 1;
                    var emotions = new List<string>() { babyStates[0].Emotion };

                    for (int i = 1; i < babyStates.Count; i++)
                    {
                        bool isAwake = babyStates[i].Awake;
                        if (isAwake != currentElementAwake)
                        {
                            babyStatesSleepIntervals.Add(new BabyStateSleepIntervalDto()
                            {
                                Awake = babyStates[i-1].Awake,
                                Emotions = emotions,
                                Percantage = ((double) countPercentage / babyStatesLength) * 100,
                                StateStarted = livestream.DateStarted.Value.AddSeconds(babyStates[i-1].AtSecond ?? 0)
                            });

                            countPercentage = 1;
                            emotions = new List<string>() { babyStates[i].Emotion };
                            currentElementAwake = isAwake;
                        }
                        else
                        {
                            emotions.Add(babyStates[i].Emotion);
                            countPercentage++;
                        }
                        
                        if (babyStates[i] != null && !string.IsNullOrEmpty(babyStates[i].Emotion))
                        {
                            string? emotion = babyStates[i].Emotion;
                            // Add emotion frequency
                            if (emotionFrequency.ContainsKey(emotion))
                            {
                                emotionFrequency[emotion]++;
                            }
                            else
                            {
                                emotionFrequency.Add(emotion, 1);
                            }
                        }
                    }

                    GetStreamStatisticsDto newLivestreamStatistics = new GetStreamStatisticsDto()
                    {
                        Id = Guid.NewGuid(),
                        StreamDuration = livestream.Time ?? 0,
                        TotalSleepDuration = totalSleepDuration,
                        BabyStates = babyStates,
                        BabyStatesSleepIntervals = babyStatesSleepIntervals,
                        EmotionFrequency = emotionFrequency
                    };

                    return newLivestreamStatistics;
                }
                throw new NotEnoughLivestreamInformationException("Not enough livestream information to generate statistics");
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (LivestreamNotFoundException)
            {
                throw;
            }
            catch (BabyNotFoundException)
            {
                throw;
            }
            catch (LivestreamNotAllowedToAccessException)
            {
                throw;
            }
            catch (NotEnoughLivestreamInformationException)
            {
                throw;
            }
        }
    }
}
