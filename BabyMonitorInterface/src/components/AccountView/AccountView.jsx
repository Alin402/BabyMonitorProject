import "./AccountView.css";
import {useEffect, useState} from "react";
import api from "../../utils/api";
import {Button, FormControl, TextField} from "@mui/material";
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import LogoutIcon from '@mui/icons-material/Logout';
const AccountView = () => {
    const [user, setUser] = useState({});
    useEffect(() => {
        const getUser = async () => {
            try {
                const res = await api.get("users/you");
                if (res.status === 200) {
                    setUser(res.data);
                }
            } catch (error) {
                console.log(error);
            }
        }
        getUser();
    }, []);

    const handleLogout = async () => {
        try {
            const res = await api.get("users/logout");
            if (res.status === 200) {
                console.log("Logged out");
                window.location = "/";
            }
        } catch (error) {
            console.log(error);
        }
    }

    return user && (
        <div className={"account-view"}>
            <div className="monitor-list-view-title-container">
                <h2 className="monitor-list-view-title">Your account:</h2>
            </div>

            <div className={"account-details"}>
                <AccountCircleIcon sx={{ fontSize: "13em", color: "whitesmoke" }} />
                <FormControl sx={{ width: "90%" }}>
                    <TextField
                        style={{ backgroundColor: "#eb5e92", borderRadius: "5px", marginTop: "1rem" }}
                        variant="outlined"
                        label="Name"
                        focused
                        value={user?.name}
                    />
                </FormControl>
                <FormControl sx={{ width: "90%" }}>
                    <TextField
                        style={{ backgroundColor: "#eb5e92", borderRadius: "5px", marginTop: "1rem" }}
                        variant="outlined"
                        label="Username"
                        focused
                        value={user?.username}
                    />
                </FormControl>
                <FormControl sx={{ width: "90%" }}>
                    <TextField
                        style={{ backgroundColor: "#eb5e92", borderRadius: "5px", marginTop: "1rem" }}
                        variant="outlined"
                        label="Username"
                        focused
                        value={user?.email}
                    />
                </FormControl>
                <FormControl sx={{ marginTop: "1rem" }}>
                    <Button variant={"outlined"} sx={{ borderColor: "#eb5e92", color: "#eb5e92" }} onClick={handleLogout}>
                        <LogoutIcon sx={{ marginRight: "3px" }} />
                        Log Out
                    </Button>
                </FormControl>
            </div>
        </div>
    )
}

export default AccountView;