import "./AddBabyAlert.css";
import Alert from "../../Alerts/Alert";
import {Button, Chip, FormControl} from "@mui/material";
import TextField from "@mui/material/TextField";
import * as React from "react";
import {useState} from "react";
import PanoramaIcon from '@mui/icons-material/Panorama';
import {readImageAsPath} from "../../../utils/readImageAsPath";
import { DateTimePicker } from "@mui/x-date-pickers/DateTimePicker";
import api from "../../../utils/api";
import dayjs from "dayjs";

const AddBabyAlert = ({ setClose, setLoading, baby, isEdit }) => {
    const [imageToUpload, setImageToUpload] = useState("");
    const [name, setName] = useState(baby?.babyName || "");
    const [date, setDate] = useState(baby?.birthdate ?? null);

    const [imageToUploadUrl, setImageToUploadUrl] = useState(baby?.babyPhoto || "");

    const handleImageChange = (e) => {
        const imageFile = e.target.files[0];
        if (imageFile) {
            setImageToUpload(imageFile);
            readImageAsPath(imageFile, (res) => {
                setImageToUploadUrl(res);
            });
        }
    }

    const handleNameChange = (e) => {
        setName(e.target.value);
    }

    const handleDateChange = (e) => {
        setDate(e);
    }

    const handleSubmitAdd = async () => {
        try {
            const formData = new FormData();
            formData.append("Name", name);
            formData.append("ImageToUpload", imageToUpload);
            formData.append("Birthdate", date);
            const res = await api.post("baby/create", formData);

            if (res.status === 200) {
                console.log(res.data);
                setLoading(true);
                setClose(false);
            }
        } catch (error) {
            console.log(error);
        }
    }

    const handleSubmitEdit = async () => {
        try {
            const formData = new FormData();
            formData.append("Name", name);
            formData.append("ImageToUpload", imageToUpload);
            formData.append("Birthdate", date);
            const res = await api.put(`baby/${baby?.babyId}`, formData);

            if (res.status === 200) {
                console.log(res.data);
                setLoading(true);
                setClose(false);
            }
        } catch (error) {
            console.log(error);
        }
    }

    return (
        <Alert
            msg={isEdit ? "Edit baby" : "Add new baby"}
            Type={"info"}
            setClose={setClose}
        >
            <FormControl>
                {
                    imageToUploadUrl ?
                    <div
                        className={"baby-card-photo"}
                        style={{
                            margin: "0 auto",
                            marginBottom: "1rem",
                            backgroundImage: `url("${imageToUploadUrl}")`,
                            backgroundSize: "cover",
                            borderRadius: "5px"
                        }}
                    ></div> :
                    <div
                        className={"baby-card-photo"}
                        style={{
                            backgroundColor: "#272c2b",
                            borderRadius: "5px",
                            display: "flex",
                            justifyContent: "center",
                            alignItems: "center",
                            margin: "0 auto",
                            marginBottom: "1rem"
                        }}
                    >
                        <PanoramaIcon sx={{ color: "whitesmoke", fontSize: "3em" }} />
                    </div>
                }
                <input style={{ display: "none" }} type={"file"} id={"imageToUpload"} accept={"image/*"} onChange={handleImageChange} />
                    <label htmlFor={"imageToUpload"} style={{ cursor: "pointer" }}>
                        <Chip label={"Add Image"} />
                    </label>
            </FormControl>
            <FormControl sx={{ width: "80%", marginTop: "1rem" }}>
                <TextField
                    label={"Name"}
                    value={name}
                    onChange={handleNameChange}
                />
            </FormControl>
            <FormControl sx={{ width: "80%", marginTop: "1rem" }}>
                <DateTimePicker value={dayjs(date)} onChange={handleDateChange} label={"Birthdate"} />
            </FormControl>
            <FormControl sx={{ width: "80%", marginTop: "1rem", marginBottom: "2rem" }}>
                <Button onClick={isEdit ? handleSubmitEdit : handleSubmitAdd} style={{width: "10rem", color: "#272c2b", borderColor: "#c9a02f", margin: "0 auto"}} variant="outlined">
                    {isEdit ? "Edit Baby" : "Add Baby"}
                </Button>
            </FormControl>
        </Alert>
    )
}

export default AddBabyAlert;