import { useEffect, useState } from 'react';
import api from '../../../utils/api';
import { Button, MenuItem, TextField, Select, FormControl, InputLabel, Divider } from '@mui/material';
import Alert from '../Alert';
import "./Specific.css";

const EditMonitoringDeviceAlert = ({ setClose, deviceId, deviceName, babyId, monitors, setMonitors, loading, setLoading }) => {
    const [babies, setBabies] = useState([]);

    const [selectedBaby, setSelectedBaby] = useState(babyId);
    const [newDeviceName, setNewDeviceName] = useState(deviceName);

    useEffect(() => {
        const getBabies = async () => {
            try {
                const res = await api.get("baby/all");
                if (res.status == 200) {
                    setBabies(res.data);
                }
            } catch (error) {
                console.log(error);
            }
        }
        getBabies();
    }, [])

    const handleEdit = async () => {
        try {
            console.log(selectedBaby)
            const res = await api.put(`device/${deviceId}`, {
                BabyId: selectedBaby.toString(),
                Name: newDeviceName
            }, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            if (res.status == 200) {
                setLoading(true);
                setClose(false);
            }
        } catch (error) {
            console.log(error);
        }
    }

    return babies && babies.length && (
        <Alert
            Type="info"
            setClose={setClose}
        > 
            <h2 style={{ fontSize: "1.2em", color: "#272c2b" }}>{`Edit device "${deviceName}"`}</h2>
            <FormControl sx={{ width: "90%" }}>
                <TextField
                    variant="outlined"
                    value={newDeviceName}
                    onChange={(e) => setNewDeviceName(e.target.value)}
                    focused
                    label="Name"
                />
            </FormControl>
            <div style={{ marginTop: "1rem" }} className="select-baby-container">
                {
                    babies && babies.length > 0 ?
                    babies.map((baby) => {
                        return (
                            <div onClick={() => setSelectedBaby(baby.id.toString())} key={baby.id} className={`select-baby ${baby.id === selectedBaby && "select-baby-selected"}`}>
                                {
                                    baby.photoUrl &&
                                    <div className="baby-photo" style={{ backgroundImage: `url("http://68.219.120.90:6060//image/${baby.photoUrl}")` }}>

                                    </div>
                                }
                                {baby.name}
                            </div>
                        )
                    }) 
                    :
                    "You have no babies"
                }
            </div>
            <Button variant="outlined" onClick={handleEdit}>
                Edit
            </Button>
        </Alert>
    )
}

export default EditMonitoringDeviceAlert;