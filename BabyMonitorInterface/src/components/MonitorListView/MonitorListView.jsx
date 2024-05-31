import { useEffect, useState } from "react";
import "./MonitorListView.css";
import axios from "axios";
import MonitorCard from "./MonitorCard/MonitorCard";
import Divider from '@mui/material/Divider';
import AddToPhotosIcon from '@mui/icons-material/AddToPhotos';
import Alert from "../Alerts/Alert";
import MobileFriendlyIcon from '@mui/icons-material/MobileFriendly';
import api from "../../utils/api";

const MonitorListView = () => {
    const [monitors, setMonitors] = useState([]);
    const [openAddMonitorAlert, setOpenAddMonitorAlert] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const request = async () => {
            try {
                const res = await api.get("device/all");

                if (res.status === 200) {
                    setMonitors(res.data);
                }
            } catch (error)
            {
                console.log(error);
            }
        }
        if (loading) {
            request().then(r => {
                setLoading(false);
            });
        }
    }, [loading]);

    return (
        <div className="monitor-list-view-container">
            <div className="monitor-list-view-title-container">
                <h2 className="monitor-list-view-title">Your monitoring devices:</h2>
            </div>
            <div className="monitor-list-view">
                {
                    monitors && monitors.length > 0 &&
                    monitors.map((monitor, index) => {
                        return (
                            <MonitorCard
                                key={monitor.id}
                                monitorId={monitor.id}
                                babyId={monitor.babyId}
                                monitorName={monitor.name}
                                babyName={monitor.babyName}
                                babyPhotoUrl={monitor.babyPhotoUrl}
                                setMonitors={setMonitors}
                                monitors={monitors}
                                loading={loading}
                                setLoading={setLoading}
                            />
                        )
                    })
                }
                <div className="add-monitor-container" onClick={() => setOpenAddMonitorAlert(true)}>
                    Add Device 
                    <AddToPhotosIcon style={{ marginLeft: "5px", fontSize: "2em" }} />                    
                </div>
            </div>
            {
                openAddMonitorAlert &&
                <Alert
                    Type="info"
                    setClose={setOpenAddMonitorAlert}
                >
                    <div style={{ display: "flex", color: "#272c2b" }}>
                        <div>
                            <h2>To add a new device, please use our app!</h2>
                            <Divider />
                            <p>Scan the <b>qr code</b> on your device.</p>
                        </div>
                        <div style={{ padding: "1rem" }}>
                            <MobileFriendlyIcon sx={{ fontSize: "5em" }} />
                        </div>
                    </div>
                </Alert>
            }
        </div>
    )
}

export default MonitorListView;