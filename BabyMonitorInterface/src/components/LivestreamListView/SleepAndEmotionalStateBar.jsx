import "./LivestreamListView.css";
import {useEffect, useState} from "react";
import {Popover} from "@mui/material";
import moment from "moment";

const SleepAndEmotionalStateBar = ({ startedAt, sleepAndEmotionsState, totalTime }) => {
    const [anchors, setAnchors] = useState(sleepAndEmotionsState.map((s) => ({
        anchor: null,
        open: false
    })));

    useEffect(() => {
        console.log(sleepAndEmotionsState);
    }, [])

    const handleClick = (event, index) => {
        let newAnchors = anchors.slice(); // Create a copy to avoid mutation
        newAnchors[index] = {
            anchor: event.currentTarget,
            open: true
        };
        setAnchors(newAnchors);
    };

    const handleClose = (index) => {
        let newAnchors = anchors.slice();
        newAnchors[index] = {
            anchor: null,
            open: false
        };
        setAnchors(newAnchors);
    };

    return (
        <div>
            <div style={{ display: "flex", alignItems: "center" }}>
                <div style={{ display: "flex", alignItems: "center" }}>
                    <div style={{ backgroundColor: "#3366cc", width: "1rem", height: "1rem", borderRadius: "5px" }}></div>
                    <p style={{ marginLeft: ".2rem", fontWeight: "700" }}>= "Awake"</p>
                </div>
                <div style={{ display: "flex", alignItems: "center", marginLeft: "1rem" }}>
                    <div style={{ backgroundColor: "#fcc93b", width: "1rem", height: "1rem", borderRadius: "5px" }}></div>
                    <p style={{ marginLeft: ".2rem", fontWeight: "700" }}>= "Asleep"</p>
                </div>
            </div>

            <div className={"sleep-and-emotional-state-bar"}>
                {
                    sleepAndEmotionsState && sleepAndEmotionsState?.length != 0 &&
                    sleepAndEmotionsState.map((state, index) => {
                        return (
                            <div
                            key={index}
                                style={{
                                    width: `${state.percantage}%`,
                                    flex: index === sleepAndEmotionsState.length - 1 && 1,
                                }}>
                                <div
                                    className={"segment"}
                                    style={{
                                        backgroundColor: state.awake ? "#3366cc" : "#fcc93b",
                                        width: "100%",
                                        position: "relative"
                                    }}
                                    onClick={(e) => handleClick(e, index)}
                                >
                                    <div style={{
                                        maxWidth: "3rem",
                                        backgroundColor: "#272c2b",
                                        padding: ".1rem",
                                        color: "#fcc93b",
                                        borderRadius: "5px",
                                        textAlign: "center",
                                        fontSize: ".7em",
                                        position: "absolute",
                                        bottom: "-2.1rem",
                                        left: "-1rem"
                                    }}>
                                        {`${moment(state.stateStarted).format('h:mm:ss A')}`}
                                    </div>
                                    {
                                        index === sleepAndEmotionsState.length - 1 &&
                                        <div style={{
                                            maxWidth: "3rem",
                                            backgroundColor: "#272c2b",
                                            padding: ".1rem",
                                            color: "#fcc93b",
                                            borderRadius: "5px",
                                            textAlign: "center",
                                            fontSize: ".7em",
                                            position: "absolute",
                                            bottom: "-2.1rem",
                                            right: "-1rem"
                                        }}>
                                            {`${moment(startedAt).add(totalTime, "seconds").format("h:mm:ss A")}`}
                                        </div>
                                    }
                                </div>
                                <Popover
                                    id={anchors[index].open ? 'simple-popover' : undefined}
                                    open={anchors[index].open}
                                    anchorEl={anchors[index].anchor}
                                    onClose={() => handleClose(index)}
                                    anchorOrigin={{
                                        vertical: 'bottom',
                                        horizontal: 'left',
                                    }}
                                    PaperProps={{
                                        style: {
                                            backgroundColor: state.awake ? "#3366cc" : "#fcc93b"
                                        }
                                    }}
                                >
                                    {
                                        state?.emotions && state?.emotions.length !== 0 &&
                                        state.emotions.map((emotion, index) => {
                                            return (
                                                <div
                                                    key={index}
                                                    className={"emotion-display-in-bar"}
                                                    style={{
                                                        color: state.awake ? "#3366cc" : "#fcc93b"
                                                    }}
                                                >
                                                    {emotion}
                                                </div>
                                            )
                                        })
                                    }
                                </Popover>
                            </div>
                        )
                    })
                }
            </div>
        </div>
    )
}

export default SleepAndEmotionalStateBar;