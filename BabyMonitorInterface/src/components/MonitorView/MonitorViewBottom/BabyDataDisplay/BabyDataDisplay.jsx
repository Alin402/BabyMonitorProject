import { useEffect, useState } from "react";
import "./BabyDataDisplay.css";
import api from "../../../../utils/api";
import TextField from '@mui/material/TextField';
import MenuItem from '@mui/material/MenuItem';
import { Button, Input, Select } from "@mui/material";
import Alert from "../../../Alerts/Alert";
import Moment from "react-moment";

const BabyDataDisplay = ({ deviceId, babyId, deviceName, setLoadingDevice }) => {
    const [babies, setBabies] = useState([]);
    const [selectedBaby, setSelectedBaby] = useState(null);
    const [showSaveButton, setShowSaveButton] = useState(false);
    const [showSuccessUpdateDevice, setShowSuccessUpdateDevice] = useState(false);

    useEffect(() => {
        const getBabies = async () => {
            try {
                const res = await api.get("baby/all");
                if (res.status === 200) {
                    setBabies(res.data);
                }
            } catch (error) {
                console.log(error);
            }
        }
        getBabies();
    }, [])

    useEffect(() => {
        if (babies.length > 0 && babyId) {
            const initialSelectedBaby = babies.find(b => b.id === babyId);
            setSelectedBaby(initialSelectedBaby);
        }
    }, [babies, babyId]);

    const handleBabyChange = (e) => {
        const selectedId = e.target.value;
        if (selectedId !== babyId) {
            setShowSaveButton(true);
        } else {
            setShowSaveButton(false);
        }

        const selectedBaby = babies.find(b => b.id === selectedId);
        setSelectedBaby(selectedBaby);
    }

    const handleUpdateDevice = async () => {
        try {
            const res = await api.put(`device/${deviceId}`, {
                name: deviceName,
                babyId: selectedBaby?.id
            });

            if (res.status === 200) {
                setLoadingDevice(true);
                setShowSaveButton(false);
                setShowSuccessUpdateDevice(true);
            }
        } catch (error) {
            console.log(error);
        }
    }

    return babies.length > 0 && selectedBaby && (
        <div className="baby-data-display">
            <div className="baby-data-display-left">
                <Select
                    style={{ backgroundColor: "#eb5e92", color: "#272c2b", fontWeight: "700" }}
                    type="select"
                    value={selectedBaby?.id}
                    label="Age"
                    sx={{ width: "20rem" }}
                    onChange={handleBabyChange}
                >
                    {babies.map((b) => (
                        <MenuItem selected={b.id === selectedBaby?.id ? true : false} key={b.id} value={b.id} sx={{ color: "#eb5e92" }}>
                            {b.name}
                        </MenuItem>
                    ))}
                </Select>
                {
                    showSaveButton &&
                    <Button onClick={handleUpdateDevice} variant="outlined" style={{ marginLeft: "1rem" }}>Save</Button>
                }
            </div>
            <div className="baby-properties">
                {
                    selectedBaby?.photoUrl &&
                    <div className="baby-picture" style={{ backgroundImage: `url("http://68.219.120.90:6060//image/${selectedBaby?.photoUrl}")` }}>
                        
                    </div>
                }
                <div style={{ marginLeft: "1rem" }}>
                    <p style={{ fontSize: "1.2em" }}>
                        {selectedBaby?.name}
                    </p>
                    <p style={{ fontSize: "1.2em" }}>
                        <Moment fromNow date={selectedBaby?.birthdate} />
                        {" was born"}
                    </p>
                </div>
            </div>
            {
                showSuccessUpdateDevice &&
                <Alert setClose={setShowSuccessUpdateDevice} Type="success" msg="Successfully updated monitoring device with new baby" />
            }
        </div>
    )
}

export default BabyDataDisplay;