import "./LivestreamListView.css";
import * as React from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import IconButton from "@mui/material/IconButton";
import KeyboardArrowDownIcon from "@mui/icons-material/KeyboardArrowDown";
import KeyboardArrowUpIcon from "@mui/icons-material/KeyboardArrowUp";
import {useState} from "react";
import Collapse from "@mui/material/Collapse";
import moment from "moment";
import api from "../../utils/api";
import LivestreamStatistics from "./LivestreamStatistics";
import InsertChartIcon from '@mui/icons-material/InsertChart';
import DateRangeIcon from '@mui/icons-material/DateRange';
import AvTimerIcon from '@mui/icons-material/AvTimer';
import DevicesIcon from '@mui/icons-material/Devices';
import ChildCareIcon from '@mui/icons-material/ChildCare';

const Row = ({ livestream, handleGetLivestreamStatistics, livestreamStatistics }) => {
    const [open, setOpen] = useState(false);
    return (
        <>
            <TableRow sx={{ '&:last-child td, &:last-child th': { outline: "0" }, border: "none" }}>
                <TableCell align="left" sx={{borderBottomColor: "transparent"}}>
                    <IconButton
                        aria-label="expand row"
                        size="small"
                        onClick={() => {
                            setOpen(!open);
                            if (!livestreamStatistics[livestream?.id]) {
                                handleGetLivestreamStatistics(livestream?.id);
                            }
                        }}
                    >
                        {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                    </IconButton>
                </TableCell>
                <TableCell align={"right"} component="th" scope="row" sx={{borderBottomColor: "transparent"}}>
                    {`${moment(livestream.dateStarted).format('MMMM Do YYYY, h:mm:ss a')}`}
                </TableCell>
                <TableCell align="right" sx={{borderBottomColor: "transparent"}}>
                    {`${moment.duration(livestream.time, 'seconds').format("h:mm:ss")}`}
                </TableCell>
                <TableCell align="right" sx={{borderBottomColor: "transparent"}}>{livestream?.device?.name}</TableCell>
                <TableCell align="right" sx={{borderBottomColor: "transparent"}}>
                    <div style={{ display: "flex", alignItems: "center", justifyContent: "right" }}>
                        {livestream?.baby.name}
                        <div
                            className={"table-display-baby-photo"}
                            style={{ backgroundImage: `url("http://68.219.120.90:6060//image/${livestream.baby.photoUrl}")`, marginLeft: ".5rem" }}
                        >
                        </div>
                    </div>
                </TableCell>
            </TableRow>
            <TableRow>
                <TableCell style={{ paddingBottom: 0, paddingTop: 0, borderBottomColor: "rgba(39, 44, 43, .3)" }} colSpan={6}>
                    <Collapse in={open} timeout="auto" unmountOnExit>
                        {
                            livestreamStatistics[livestream?.id] ?
                                <LivestreamStatistics startedAt={livestream?.dateStarted}npm livestreamStatistics={livestreamStatistics[livestream?.id]} /> :
                                <h2>No statistics to display...</h2>
                        }
                    </Collapse>
                </TableCell>
            </TableRow>

        </>
    )
}

const LivestreamListTable = ({ livestreams }) => {
    const [livestreamStatistics, setLivestreamStatistics] = useState({});

    const handleGetLivestreamStatistics = async (id) => {
        try {
            const res = await api.get(`livestream/${id}/statistics`);

            if (res.status === 200) {
                console.log(res.data);
                setLivestreamStatistics({
                   ...livestreamStatistics,
                   [id]: res.data
                });
            }
        } catch (error) {
            console.log(error);
        }
    }

    return (
        <TableContainer component={Paper} style={{ backgroundColor: "#eb5e92" }}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        <TableCell align="left" sx={{ fontWeight: "700", borderBottomColor: "rgba(39, 44, 43, .3)" }}>
                            <InsertChartIcon sx={{ marginBottom: "-.4rem", marginRight: ".2rem" }} />
                            Statistics
                        </TableCell>
                        <TableCell align={"right"} sx={{ fontWeight: "700", borderBottomColor: "rgba(39, 44, 43, .3)" }}>
                            <DateRangeIcon sx={{ marginBottom: "-.4rem", marginRight: ".2rem" }} />
                            Started At
                        </TableCell>
                        <TableCell align="right" sx={{ fontWeight: "700", borderBottomColor: "rgba(39, 44, 43, .3)" }}>
                            <AvTimerIcon sx={{ marginBottom: "-.4rem", marginRight: ".2rem" }} />
                            Duration
                        </TableCell>
                        <TableCell align="right" sx={{ fontWeight: "700", borderBottomColor: "rgba(39, 44, 43, .3)" }}>
                            <DevicesIcon sx={{ marginBottom: "-.4rem", marginRight: ".2rem" }} />
                            Device
                        </TableCell>
                        <TableCell align="right" sx={{ fontWeight: "700", borderBottomColor: "rgba(39, 44, 43, .3)" }}>
                            <ChildCareIcon sx={{ marginBottom: "-.4rem", marginRight: ".2rem" }} />
                            Baby
                        </TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {livestreams.map((livestream, index) => (
                        <Row
                            key={index}
                            livestream={livestream}
                            handleGetLivestreamStatistics={handleGetLivestreamStatistics}
                            livestreamStatistics={livestreamStatistics}
                        />
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}

export default LivestreamListTable;