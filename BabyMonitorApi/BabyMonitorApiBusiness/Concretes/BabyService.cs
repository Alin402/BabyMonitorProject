using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.Dtos.Babies;
using BabyMonitorApiDataAccess.Entities;
using BabyMonitorApiDataAccess.Repositories.Abstractions;
using BabyMonitorApiDataAccess.CustomValidationExceptions;

namespace BabyMonitorApiBusiness.Concretes
{
    public class BabyService : IBabyService
    {
        private const string FILE_SERVER_URL = "http://68.219.120.90:6060/upload"; 

        private readonly IRepository<Baby> _babyRepository;
        private readonly IUserRepository _userRepository;

        public BabyService(IRepository<Baby> babyRepository, IUserRepository userRepository)
        {
            _babyRepository = babyRepository;
            _userRepository = userRepository;
        }

        public async Task<CreatedBabyDto> CreateBaby(CreateBabyDto createBaby, string userEmail)
        {
            try
            {
                // Find connected user
                User? user = await _userRepository.FindByEmailAsync(userEmail);

                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                Baby newBaby = new Baby()
                {
                    Id = Guid.NewGuid(),
                    Name = createBaby.Name,
                    UserId = user.Id,
                    BirthDate = DateTime.Parse(createBaby.BirthDate),
                };

                if (createBaby.ImageToUpload != null)
                {
                    // Upload image to the server and save its path
                    using (HttpClient httpClient = new())
                    {
                        using (MemoryStream ms = new())
                        {
                            await createBaby.ImageToUpload.CopyToAsync(ms);
                            byte[] imageBytes = ms.ToArray();

                            var content = new MultipartFormDataContent();

                            content.Add(new ByteArrayContent(imageBytes), "imageToUpload", createBaby.ImageToUpload.FileName);
                            var response = await httpClient.PostAsync(FILE_SERVER_URL, content);

                            if (!response.IsSuccessStatusCode)
                            {
                                throw new UploadBabyPictureException("Failed to upload baby picture");
                            }

                            newBaby.PhotoUrl = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                Baby? createdBaby =  await _babyRepository.PostAsync(newBaby);
                if (createdBaby != null)
                {
                    return new CreatedBabyDto()
                    {
                        Id = createdBaby.Id,
                        Name = createdBaby.Name,
                        UserId=createdBaby.UserId,
                        PhotoUrl=createdBaby.PhotoUrl,
                    };
                }
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (UploadBabyPictureException)
            {
                throw;
            }
            return null;
        }

        public async Task DeleteBaby(Guid id, string userEmail)
        {
            try
            {
                Baby? foundBaby = await _babyRepository.GetByIdAsync(id);

                if (foundBaby != null)
                {
                    User? connectedUser = await _userRepository.FindByEmailAsync(userEmail);
                    if (connectedUser == null)
                    {
                        throw new UserNotFoundException("User not found");
                    }

                    if (!foundBaby.UserId.Equals(connectedUser.Id))
                    {
                        throw new BabyNotAllowedToAccessException("Baby not allowed to access");
                    }

                    await _babyRepository.DeleteAsync(id);
                }
                else
                {
                    throw new BabyNotFoundException("Baby not found");
                }
            }
            catch (BabyNotFoundException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (BabyNotAllowedToAccessException)
            {
                throw;
            }
        }

        public async Task<List<CreatedBabyDto>> GetAllBabies(string userEmail)
        {
            try
            {
                // Find connected user
                User? user = await _userRepository.FindByEmailAsync(userEmail);

                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                return _babyRepository.
                    GetAllAsync().
                    Result.
                    ToList().
                    Where(b => b.UserId == user.Id).
                    ToList().
                    Select(b => new CreatedBabyDto()
                    {
                        Id = b.Id,
                        UserId = b.UserId,
                        PhotoUrl = b.PhotoUrl,
                        Name = b.Name,
                        Birthdate = b.BirthDate
                    }).
                    ToList();
            }
            catch (UserNotFoundException)
            {
                throw;
            }
        }

        public async Task<CreatedBabyDto> GetBaby(Guid id, string userEmail)
        {
            try
            {
                Baby? foundBaby = await _babyRepository.GetByIdAsync(id);

                if (foundBaby != null)
                {
                    User? connectedUser = await _userRepository.FindByEmailAsync(userEmail);
                    if (connectedUser == null)
                    {
                        throw new UserNotFoundException("User not found");
                    }

                    if (!foundBaby.UserId.Equals(connectedUser.Id))
                    {
                        throw new BabyNotAllowedToAccessException("Baby not allowed to access");
                    }

                    return new CreatedBabyDto() 
                    { 
                        Id = foundBaby.Id,
                        Name = foundBaby.Name,
                        UserId = foundBaby.UserId,
                        PhotoUrl = foundBaby.PhotoUrl
                    };
                }
                throw new BabyNotFoundException("Baby not found");
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (BabyNotAllowedToAccessException)
            {
                throw;
            }
            catch (BabyNotFoundException)
            {
                throw;
            }
            return null;
        }

        public async Task<CreatedBabyDto> UpdateBaby(Guid id, UpdateBabyDto updateBaby, string userEmail)
        {
            try
            {
                Baby? foundBaby = await _babyRepository.GetByIdAsync(id);

                if (foundBaby != null)
                {
                    User? connectedUser = await _userRepository.FindByEmailAsync(userEmail);
                    if (connectedUser == null)
                    {
                        throw new UserNotFoundException("User not found");
                    }

                    if (!foundBaby.UserId.Equals(connectedUser.Id))
                    {
                        throw new BabyNotAllowedToAccessException("Baby not allowed to access");
                    }

                    foundBaby.Name = updateBaby.Name;
                    foundBaby.BirthDate = DateTime.Parse(updateBaby.BirthDate);

                    if (updateBaby.ImageToUpload != null)
                    {
                        // Upload image to the server and save its path
                        using (HttpClient httpClient = new())
                        {
                            using (MemoryStream ms = new())
                            {
                                await updateBaby.ImageToUpload.CopyToAsync(ms);
                                byte[] imageBytes = ms.ToArray();

                                var content = new MultipartFormDataContent();

                                content.Add(new ByteArrayContent(imageBytes), "imageToUpload", updateBaby.ImageToUpload.FileName);
                                var response = await httpClient.PostAsync(FILE_SERVER_URL, content);

                                if (!response.IsSuccessStatusCode)
                                {
                                    throw new UploadBabyPictureException("Failed to upload baby picture");
                                }

                                foundBaby.PhotoUrl = await response.Content.ReadAsStringAsync();
                            }
                        }
                    }

                    Baby? updatedBaby = await _babyRepository.UpdateAsync(foundBaby);
                    if (updatedBaby != null)
                    {
                        return new CreatedBabyDto()
                        {
                            Id = updatedBaby.Id,
                            Name = updatedBaby.Name,
                            UserId = updatedBaby.UserId,
                            PhotoUrl = updatedBaby.PhotoUrl
                        };
                    }
                }
                else
                {
                    throw new BabyNotFoundException("Baby not found");
                }
            }
            catch (BabyNotFoundException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (UploadBabyPictureException)
            {
                throw;
            }
            catch (BabyNotAllowedToAccessException)
            {
                throw;
            }
            return null;
        }
    }
}
