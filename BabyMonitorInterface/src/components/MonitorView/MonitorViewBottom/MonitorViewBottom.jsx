import "./MonitorViewBottom.css";
import Grid from "@mui/material/Unstable_Grid2/Grid2";
import { styled } from '@mui/material/styles';
import Paper from '@mui/material/Paper';
import { Height } from "@mui/icons-material";
import useMediaQuery from '@mui/material/useMediaQuery';
import { useTheme } from "@mui/material/styles";
import TemperatureData from "./TemperatureData/TemperatureData";
import LivestreamPlayer from "./LivestreamPlayer/LivestreamPlayer";
import { Divider } from "@mui/material";
import ANGRY from "../../../emotions/ANGRY.png";
import CALM from "../../../emotions/CALM.png";
import CONFUSED from "../../../emotions/CONFUSED.png";
import DISGUSTED from "../../../emotions/DISGUSTED.png";
import FEAR from "../../../emotions/FEAR.png";
import HAPPY from "../../../emotions/HAPPY.png";
import SAD from "../../../emotions/SAD.png";
import SURPRISED from "../../../emotions/SURPRISED.png";
import NO_EMOTION from "../../../emotions/NO_EMOTION.png";
import BabyDataDisplay from "./BabyDataDisplay/BabyDataDisplay";

const emotionToImage = {
    "ANGRY": ANGRY,
    "CALM": CALM,
    "CONFUSED": CONFUSED,
    "DISGUSTED": DISGUSTED,
    "FEAR": FEAR,
    "HAPPY": HAPPY,
    "SAD": SAD,
    "SURPRISED": SURPRISED
}

const Item = styled(Paper)(({ theme }) => ({
    ...theme.typography.body2,
    padding: theme.spacing(1),
    borderRadius: "10px",
    color: theme.palette.text.secondary,
  }));

const MonitorViewBottom = ({ temperatureData, emotions, awake, boundingBox, playerRef, boundingBoxRef, monitoringDeviceActive, livestreamStreaming, device, setLoadingDevice }) => {
    const theme = useTheme();
    const isSmallScreen = useMediaQuery(theme.breakpoints.down('sm'));

    return device && (
        <div className="monitor-view-bottom">
            <Grid container rowSpacing={1} columnSpacing={{ xs: 1, sm: 2, md: 3 }} style={{ height: "90%" }}>
                <Grid item xs={12} sm={7}>
                    <Item style={{ height: isSmallScreen ? "60vh" : "95%", backgroundColor: "transparent", boxShadow: "none" }} >
                        <LivestreamPlayer
                            boundingBox={boundingBox}
                            playerRef={playerRef}
                            boundingBoxRef={boundingBoxRef}
                            monitoringDeviceActive={monitoringDeviceActive}
                            livestreamStreaming={livestreamStreaming}
                            device={device}
                         />
                    </Item>
                    <Item style={{ marginTop: "1rem", height: isSmallScreen ? "22vh" : "20%", backgroundColor: "transparent", boxShadow: "none" }}>
                        {
                            device.baby &&
                            <BabyDataDisplay deviceId={device.id} babyId={device?.baby?.id} deviceName={device?.name} setLoadingDevice={setLoadingDevice} />
                        }
                    </Item>
                </Grid>
                <Grid item xs={12} sm={5}>
                    <Item style={{ height: isSmallScreen ? "20rem" : "85%", backgroundColor: "transparent", boxShadow: "none" }}>
                        <h2 className="baby-emotions-title">Your baby is felling...</h2>
                        <Divider style={{ borderColor: "#eb5e92" }} />
                        <div className="emotion-display">
                            {
                                emotions[0]?
                                <>
                                    {/*<img className="emotion-image" src={emotionToImage[emotions[0]]} />*/}
                                    <h2 style={{ color: "#eb5e92" }}>
                                    {
                                        emotions[0]
                                    }
                                    </h2>
                                </> :
                                <>
                                    {/*<img className="emotion-image" src={NO_EMOTION} />*/}
                                    <h2 style={{ color: "#eb5e92" }}>
                                    {
                                        "No emotion to display..."
                                    }
                                    </h2>
                                </>
                            }
                            <h2 style={{ color: "#eb5e92" }}>
                                {
                                    awake === null ? "Unknown" : awake ? "Awake" : "Asleep"
                                }
                            </h2>
                        </div>
                    </Item>
                    <Item style ={{ marginTop: "1rem", height: "30%", backgroundColor: "transparent", boxShadow: "none" }}>
                        <TemperatureData 
                            temperatureData={temperatureData}
                        />
                    </Item>
                </Grid>
            </Grid>
        </div>
    );

}

export default MonitorViewBottom;