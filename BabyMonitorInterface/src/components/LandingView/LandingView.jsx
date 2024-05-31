import "./LandingView.css";
import Box from "@mui/material/Box";
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import HistoryIcon from '@mui/icons-material/History';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import DevicesIcon from '@mui/icons-material/Devices';
import ChildCareIcon from '@mui/icons-material/ChildCare';
import {Link} from "react-router-dom";

const LandingView = () => {
    return (
        <div className={"landing-view"}>
            <Link to={"/account"} style={{ textDecoration: "none" }}>
                <Box sx={{ boxShadow: 1 }} className={"landing-card"}>
                    <div className={"overlay-container"}>
                        <h2 className={"overlay-text"}>Go</h2>
                    </div>
                    <div className={"landing-card-top"}>
                        <div className={"dot"} style={{ backgroundColor: "#eb5e92" }}>
                        </div>
                        <h2 className={"card-title"}>Your account</h2>
                    </div>
                    <div className={"landing-card-bottom"}>
                        <div className={"go-container"}>
                            <AccountBoxIcon sx={{ color: "#fcc93b", fontSize: "3.5em" }} />
                        </div>
                    </div>
                </Box>
            </Link>
            <Link to={"/monitors"} style={{ textDecoration: "none" }}>
                <Box sx={{ boxShadow: 1 }} className={"landing-card"}>
                    <div className={"overlay-container"}>
                        <h2 className={"overlay-text"}>Go</h2>
                    </div>
                    <div className={"landing-card-top"}>
                        <div className={"dot"} style={{ backgroundColor: "#fcc93b" }}>
                        </div>
                        <h2 className={"card-title"}>Your monitoring devices</h2>
                    </div>
                    <div className={"landing-card-bottom"}>
                        <div className={"go-container"}>
                            <DevicesIcon sx={{ color: "#fcc93b", fontSize: "3.5em" }} />
                        </div>
                    </div>
                </Box>
            </Link>
            <Link to={"/babies"} style={{ textDecoration: "none" }}>
                <Box sx={{ boxShadow: 1 }} className={"landing-card"}>
                    <div className={"overlay-container"}>
                        <h2 className={"overlay-text"}>Go</h2>
                    </div>
                    <div className={"landing-card-top"}>
                        <div className={"dot"} style={{ backgroundColor: "#fcc93b" }}>
                        </div>
                        <h2 className={"card-title"}>Your babies</h2>
                    </div>
                    <div className={"landing-card-bottom"}>
                        <div className={"go-container"}>
                            <ChildCareIcon sx={{ color: "#fcc93b", fontSize: "3.5em" }} />
                        </div>
                    </div>
                </Box>
            </Link>
            <Link to={"/livestreams"} style={{ textDecoration: "none" }}>
                <Box sx={{ boxShadow: 1 }} className={"landing-card"}>
                    <div className={"overlay-container"}>
                        <h2 className={"overlay-text"}>Go</h2>
                    </div>
                    <div className={"landing-card-top"}>
                        <div className={"dot"} style={{ backgroundColor: "#eb5e92" }}>
                        </div>
                        <h2 className={"card-title"}>Your livestream history</h2>
                    </div>
                    <div className={"landing-card-bottom"}>
                        <div className={"go-container"}>
                            <HistoryIcon sx={{ color: "#fcc93b", fontSize: "3.5em" }} />
                        </div>
                    </div>
                </Box>
            </Link>
        </div>
    )
}

export default LandingView;