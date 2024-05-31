import * as React from 'react';
import { styled } from '@mui/material/styles';
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardMedia from '@mui/material/CardMedia';
import CardContent from '@mui/material/CardContent';
import CardActions from '@mui/material/CardActions';
import Collapse from '@mui/material/Collapse';
import Avatar from '@mui/material/Avatar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import { red } from '@mui/material/colors';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ShareIcon from '@mui/icons-material/Share';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { Link } from 'react-router-dom';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { useState } from 'react';
import Alert from '../../Alerts/Alert';
import { Button, Divider } from '@mui/material';
import ContactSupportIcon from '@mui/icons-material/ContactSupport';
import axios from 'axios';
import ChoiceAlert from '../../Alerts/Generic/ChoiceAlert';
import api from '../../../utils/api';
import { TextField } from '@mui/material';
import EditMonitoringDeviceAlert from '../../Alerts/Specific/EditMonitoringDeviceAlert';

const MonitorCard = ({ monitorId, monitorName, babyName, babyPhotoUrl, setMonitors, monitors, babyId, loading, setLoading }) => {
    const [anchorEl, setAnchorEl] = React.useState(null);
    const open = Boolean(anchorEl);

    const [openDeleteMonitorAlert, setOpenDeleteMonitorAlert] = React.useState(false);
    const [openSuccessDeleteMonitor, setOpenSuccessDeleteMonitor] = React.useState(false);
    const [openEditMonitor, setOpenEditMonitor] = React.useState(false);

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleDeleteMonitoringDevice = () => {
        setOpenDeleteMonitorAlert(true);
        handleClose();
    }

    const handleEditMonitoringDevice = () => {
        setOpenEditMonitor(true);
        handleClose();
    }

    const handleDelete = async () => {
        try {
            const res = await api.delete(`device/${monitorId}`);

            if (res.status === 200) {
                setOpenSuccessDeleteMonitor(true);
                setMonitors(monitors.filter((monitor) => monitor.id !== monitorId));
            }
        } catch (error)
        {
            console.log(error);
        }
    }

  return (
    <>
        <Card sx={{ maxWidth: 345, backgroundColor: "#eb5e92", marginRight: "2rem", marginBottom: "2rem" }}>
        <CardHeader
            action={
            <IconButton 
                id="basic-button"
                aria-controls={open ? 'basic-menu' : undefined}
                aria-haspopup="true"
                aria-expanded={open ? 'true' : undefined}
                onClick={handleClick}
            >
                <MoreVertIcon />
            </IconButton>
            }
            title={<Link style={{ textDecoration: "none", color: "#272c2b", fontWeight: "700" }} to={`/monitor/${monitorId}`}>{monitorName}</Link>}
            subheader={babyName}
        />
        <Menu
            id="basic-menu"
            anchorEl={anchorEl}
            open={open}
            onClose={handleClose}
            MenuListProps={{
                'aria-labelledby': 'basic-button',
            }}
        >
            <MenuItem onClick={handleDeleteMonitoringDevice}>Delete <DeleteIcon /></MenuItem>
            <MenuItem onClick={handleEditMonitoringDevice}>Edit<EditIcon /></MenuItem>
        </Menu>
        {
            babyPhotoUrl ?
            <CardMedia
                component="img"
                height="194"
                image={`http://68.219.120.90:6060/image/${babyPhotoUrl}`}
                alt="Paella dish"
            />:
            <CardMedia
                component="img"
                height="194"
                image={`https://i.pinimg.com/564x/74/42/9c/74429c9d0c57594d11c01b4747e2ca94.jpg`}
                alt="Paella dish"
            />
        }
        <CardContent>
        </CardContent>
        </Card>
        {
            openDeleteMonitorAlert &&
            <ChoiceAlert
                setClose={setOpenDeleteMonitorAlert}
                msg={`Are you sure you want to delete device "${monitorName}"?`}
                handleYes={handleDelete}
            />
        }
        {
            openSuccessDeleteMonitor &&
            <Alert Type="success" msg="Successfully deleted baby!" setClose={setOpenSuccessDeleteMonitor} />
        }
        {
            openEditMonitor &&
            <EditMonitoringDeviceAlert 
                setClose={setOpenEditMonitor}
                deviceId={monitorId}
                deviceName={monitorName}
                babyId={babyId}
                monitors={monitors}
                setMonitors={setMonitors}
                loading={loading}
                setLoading={setLoading}
            />  
        }
    </>
  );
}

export default MonitorCard;