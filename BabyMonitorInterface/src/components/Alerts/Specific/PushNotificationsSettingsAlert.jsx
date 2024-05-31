import Alert from "../Alert";
import "./Specific.css";
import {Button, Checkbox, Divider, FormControlLabel} from "@mui/material";
import {useState} from "react";

const options = {
    "CALM": false,
    "DISGUSTED": false,
    "CONFUSED": false,
    "HAPPY": false,
    "SAD": false,
    "SURPRISED": false,
    "ANGRY": false,
    "FEAR": false,
    "AWAKE": false,
    "ASLEEP": false
}
const PushNotificationsSettingsAlert = ({ setClose, socket, user, deviceId }) => {
    const [notifOptions, setNotifOptions] = useState(JSON.parse(localStorage.getItem("notifOptions")) || options);

    const handleSubmit = async () => {
        localStorage.setItem("notifOptions", JSON.stringify(notifOptions))

        const message = {
            ClientType: 2,
            MessageType: 8,
            Content: {
                UserId: user.id,
                DeviceId: deviceId,
                NotificationsOptions: notifOptions
            }
        }

        await socket.send(JSON.stringify(message));
        setClose(false)
    }
    return (
        <Alert
            Type={"info"}
            setClose={setClose}
        >
            <div style={{ padding: "1rem" }}>
                <p className={"options-title"}>
                    You will be notified when the baby has
                    the <br />
                    following states:
                </p>
                <Divider />
                <div className={"options-container"} style={{ margin: "1rem 0" }}>
                    {
                        Object.keys(notifOptions).map((o, index) => {
                            return (
                                <FormControlLabel
                                    control={
                                    <Checkbox
                                        title={"sex"}
                                        value={o}
                                        checked={notifOptions[o]}
                                        onChange={(e) => {
                                            setNotifOptions({
                                                ...notifOptions,
                                                [o] : !notifOptions[o]
                                            })
                                        }}
                                    />}
                                    label={o}
                                />
                            )
                        })
                    }
                </div>
                <Button variant={"outlined"} sx={{ color: "#272c2b", borderColor: "#272c2b" }} onClick={handleSubmit}>Submit</Button>
            </div>
        </Alert>
    )
}

export default PushNotificationsSettingsAlert;