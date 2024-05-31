import "./MonitorViewTop.css";
import React from "react";
import DeviceThermostatIcon from '@mui/icons-material/DeviceThermostat';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import WifiIcon from '@mui/icons-material/Wifi';
import {Button, Divider, FormControl, TextField} from "@mui/material";
import { useState } from "react";
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import IconButton from '@mui/material/IconButton';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import RestartAltIcon from '@mui/icons-material/RestartAlt';
import DisplaySettingsIcon from '@mui/icons-material/DisplaySettings';
import Alert from "../../Alerts/Alert";
import ConnectToWifiAlert from "../../Alerts/Specific/ConnectToWifiAlert";
import WifiOffIcon from '@mui/icons-material/WifiOff';
import PushNotificationsSettingsAlert from "../../Alerts/Specific/PushNotificationsSettingsAlert";
import CircleNotificationsIcon from '@mui/icons-material/CircleNotifications';

const MonitorViewTop = ({monitoringDeviceActive, systemData, deviceName, socket, deviceId, user}) => {
    const [anchorEl, setAnchorEl] = React.useState(null);
    const open = Boolean(anchorEl);

    const [anchorEl2, setAnchorEl2] = React.useState(null);
    const open2 = Boolean(anchorEl2);
    
    const [dropdownOpen, setDropDownOpen] = useState(false);

    const [showWifiAlert, setShowWifiAlert] = useState(false);

    const [showPushNotificationsSettingsAlert, setShowPushNotificationsSettingsAlert] = useState(false);
    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleClick2 = (event) => {
        setAnchorEl2(event.currentTarget);
    };

    const handleClose2 = () => {
        setAnchorEl2(null);
    };

    const handleRestartDevice = () => {
        if (socket) {
            console.log(deviceId)
            const message = {
                MessageType: 8,
                Content: {
                    UserId: "3604F6D9-48B7-4C16-8347-E0A3BCBD99C1",
                    DeviceId: deviceId
                }
            }
            socket.send(JSON.stringify(message));
        }
    }

    const handleOpenWifiAlert = () => {
        setShowWifiAlert(!showWifiAlert);
        setAnchorEl2(false);
    }
    
    return(
        <div className="monitor-view-top">
            <div className="monitor-view-top-left">
                <h2 className="monitor-view-top-title">{deviceName}</h2>
                <div className="monitor-status" style={{ backgroundColor: monitoringDeviceActive ? "#269f67" : "#c8224d" }}>
                    {
                        monitoringDeviceActive ? "Online" : "Offline"
                    }
                    {
                        monitoringDeviceActive?
                            <WifiIcon style={{ marginBottom: '-5px' }} /> :
                            <WifiOffIcon style={{ marginBottom: '-5px' }} />
                    }
                </div>
            </div>
            <div className="monitor-view-top-right">
                <div>
                    <IconButton
                        onClick={() => {
                            setShowPushNotificationsSettingsAlert(true)
                        }}
                    >
                        <CircleNotificationsIcon sx={{ color: "#272c2b", fontSize: "1.2em" }} />
                    </IconButton>
                </div>
                <div>
                    <IconButton 
                        id="basic-button"
                        aria-controls={open2 ? 'basic-menu' : undefined}
                        aria-haspopup="true"
                        aria-expanded={open2 ? 'true' : undefined}
                        onClick={handleClick2}
                    >
                        <WifiIcon sx={{ color: "#272c2b" }} />
                    </IconButton>
                    <Menu
                        id="basic-menu"
                        anchorEl={anchorEl2}
                        open={open2}
                        onClose={handleClose2}
                    >
                        <p style={{ backgroundColor: "#272c2b", color: "#fcc93b", padding: ".5rem .5rem" }}>{systemData.wifiName}</p>
                        <MenuItem onClick={handleOpenWifiAlert}>Connect</MenuItem>
                    </Menu>
                </div>
                <div style={{ marginRight: "2rem" }}>
                    <IconButton 
                        id="basic-button"
                        aria-controls={open ? 'basic-menu' : undefined}
                        aria-haspopup="true"
                        aria-expanded={open ? 'true' : undefined}
                        onClick={handleClick}
                    >
                        <DisplaySettingsIcon sx={{ color: "#272c2b" }} />
                    </IconButton>
                    <Menu
                        id="basic-menu"
                        anchorEl={anchorEl}
                        open={open}
                        onClose={handleClose}
                    >
                        <MenuItem onClick={handleRestartDevice}><RestartAltIcon sx={{ marginRight: "3px" }} /> Restart device</MenuItem>
                    </Menu>
                </div>
                <h2 className="system-temperature-display">{systemData.systemTemperature.toFixed(2)} <DeviceThermostatIcon style={{ marginBottom: '-5px' }}/></h2>
            </div>
            {
                showPushNotificationsSettingsAlert &&
                <PushNotificationsSettingsAlert
                    setClose={setShowPushNotificationsSettingsAlert}
                    socket={socket}
                    user={user}
                    deviceId={deviceId}
                />
            }
            {
                showWifiAlert &&
                <ConnectToWifiAlert
                    setClose={setShowWifiAlert}
                    deviceId={deviceId}
                    socket={socket}
                />
            }
        </div>
    )
}

export default MonitorViewTop;