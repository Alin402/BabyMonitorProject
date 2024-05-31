import { useEffect, useState } from 'react';
import api from '../../../utils/api';
import { Button, MenuItem, TextField, Select, FormControl, InputLabel, Divider } from '@mui/material';
import Alert from '../Alert';
import "./Specific.css";

const ConnectToWifiAlert = ({ setClose, deviceId, deviceName, babyId, socket }) => {
    const [wifiName, setWifiName] = useState("");
    const [wifiPassword, setWifiPassword] = useState("");

    const handleConnectToWifi = () => {
        if (wifiName && wifiPassword) {
            if (socket) {
                const message = {
                    MessageType: 9,
                    Content: {
                        UserId: "3604F6D9-48B7-4C16-8347-E0A3BCBD99C1",
                        DeviceId: deviceId,
                        WifiName: wifiName,
                        WifiPassword: wifiPassword
                    }
                }
                socket.send(JSON.stringify(message));
            }
        }
    }

    return (
        <Alert
            Type="info"
            setClose={setClose}
        >
            <h2 style={{ fontSize: "1.2em", color: "#272c2b" }}>Connect to Wifi</h2>
            <TextField
                variant={"outlined"}
                sx={{ width: "90%", marginTop: "1rem" }}
                label={"Wifi Name"}
                value={wifiName}
                onChange={(e) => setWifiName(e.target.value)}
            />
            <TextField
                type={"password"}
                variant={"outlined"}
                sx={{ width: "90%", marginTop: "1rem" }}
                label={"Wifi Password"}
                value={wifiPassword}
                onChange={(e) => setWifiPassword(e.target.value)}
            />
            <Button variant="outlined" sx={{ marginTop: "1rem" }} onClick={handleConnectToWifi}>
                Connect
            </Button>
        </Alert>
    )
}

export default ConnectToWifiAlert;