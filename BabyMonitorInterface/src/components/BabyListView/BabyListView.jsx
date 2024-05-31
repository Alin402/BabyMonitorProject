import "./BabyListView.css";
import { useEffect, useState } from "react";
import axios from "axios";
import Divider from '@mui/material/Divider';
import AddToPhotosIcon from '@mui/icons-material/AddToPhotos';
import Alert from "../Alerts/Alert";
import MobileFriendlyIcon from '@mui/icons-material/MobileFriendly';
import api from "../../utils/api";
import BabyCard from "./BabyCard/BabyCard";
import AddReactionIcon from '@mui/icons-material/AddReaction';
import AddBabyAlert from "./AddBabyAlert/AddBabyAlert";
const BabyListView = () => {
    const [babies, setBabies] = useState([]);
    const [openAddBabyAlert, setOpenAddBabyAlert] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const request = async () => {
            try {
                const res = await api.get("baby/all");

                if (res.status === 200) {
                    setBabies(res.data);
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
        <div className={"baby-list-view-container"}>
            <div className="monitor-list-view-title-container">
                <h2 className="monitor-list-view-title">Your babies:</h2>
            </div>
            <div className={"baby-list-view"}>
                {
                    babies && babies.length !== 0 &&
                    babies.map((baby, index) => {
                        return (
                            <BabyCard
                                key={index}
                                babyId={baby.id}
                                babyName={baby.name}
                                babyPhoto={baby?.photoUrl}
                                babyBirthDate={baby?.birthdate}
                                setLoading={setLoading}
                            />
                        )
                    })
                }
                <div className="add-monitor-container" style={{ padding: ".3rem" }} onClick={() => setOpenAddBabyAlert(true)}>
                    <AddReactionIcon style={{ fontSize: "3em" }} />
                </div>
                {
                    openAddBabyAlert &&
                    <AddBabyAlert
                        setClose={setOpenAddBabyAlert}
                        setLoading={setLoading}
                    />
                }
            </div>
        </div>
    )
}

export default BabyListView;