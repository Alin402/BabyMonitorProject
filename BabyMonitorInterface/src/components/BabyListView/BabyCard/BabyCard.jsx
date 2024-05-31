import "./BabyCard.css";
import BabyChangingStationIcon from '@mui/icons-material/BabyChangingStation';
import * as React from "react";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import IconButton from "@mui/material/IconButton";
import MenuItem from "@mui/material/MenuItem";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import Menu from "@mui/material/Menu";
import Moment from "react-moment";
import {useState} from "react";
import ChoiceAlert from "../../Alerts/Generic/ChoiceAlert";
import api from "../../../utils/api";
import AddBabyAlert from "../AddBabyAlert/AddBabyAlert";
const BabyCard = ({ babyId, babyName, babyPhoto, babyBirthDate, setLoading }) => {
    const [anchorEl, setAnchorEl] = React.useState(null);
    const open = Boolean(anchorEl);

    const [openDeleteBabyAlert, setOpenDeleteBabyAlert] = useState(false);
    const [openEditBabyAlert, setOpenEditBabyAlert] = useState(false);

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleDeleteBabyAlert = () => {
        setOpenDeleteBabyAlert(true);
        setAnchorEl(null);
    }

    const handleDeleteBaby = async () => {
        try {
            const res = await api.delete(`baby/${babyId}`);
            if (res.status === 200) {
                console.log(res.data);
                setOpenDeleteBabyAlert(false);
                setLoading(true);
            }
        } catch (error) {
            console.log(error);
        }
    }

    const handleEditBabyAlert = () => {
        setOpenEditBabyAlert(true);
        setAnchorEl(null);
    }

    return (
        <div className={"baby-card"}>
            {
                babyPhoto ?
                <div
                    className={"baby-card-photo"}
                    style={{ backgroundImage: `url("http://68.219.120.90:6060/image/${babyPhoto}")` }}
                ></div> :
                <div
                    className={"baby-card-photo"}
                    style={{ backgroundImage: "url(https://i.pinimg.com/564x/74/42/9c/74429c9d0c57594d11c01b4747e2ca94.jpg)" }}
                ></div>
            }
            <div className={"baby-card-info"} style={{ display: "flex", alignItems: "center" }}>
                <div style={{ marginRight: "1rem", marginBottom: "-.3rem" }}>
                    <BabyChangingStationIcon sx={{ fontSize: "2em" }} />
                </div>
                <div>
                    <div>
                        {babyName}
                    </div>
                    <div className={"baby-card-info-age"}>
                        <Moment fromNow date={babyBirthDate} />
                        {" was born"}
                    </div>
                </div>
            </div>
            <div>
                <IconButton
                    id="basic-button"
                    aria-controls={open ? 'basic-menu' : undefined}
                    aria-haspopup="true"
                    aria-expanded={open ? 'true' : undefined}
                    onClick={handleClick}
                >
                    <MoreVertIcon />
                </IconButton>
                <Menu
                    id="basic-menu"
                    anchorEl={anchorEl}
                    open={open}
                    onClose={handleClose}
                    MenuListProps={{
                        'aria-labelledby': 'basic-button',
                    }}
                >
                    <MenuItem onClick={handleDeleteBabyAlert}>Delete <DeleteIcon /></MenuItem>
                    <MenuItem onClick={handleEditBabyAlert}>Edit<EditIcon /></MenuItem>
                </Menu>
            </div>
            {
                openDeleteBabyAlert &&
                <ChoiceAlert
                    setClose={setOpenDeleteBabyAlert}
                    msg={`Are you sure you want to delete baby "${babyName}"? :(`}
                    handleYes={handleDeleteBaby}
                />
            }
            {
                openEditBabyAlert &&
                <AddBabyAlert
                    setClose={setOpenEditBabyAlert}
                    setLoading={setLoading}
                    isEdit={true}
                    baby={{
                        babyId,
                        babyName,
                        babyPhoto: `http://68.219.120.90:6060//image/${babyPhoto}`,
                        birthdate: babyBirthDate
                    }}
                />
            }
        </div>
    )
}

export default BabyCard;