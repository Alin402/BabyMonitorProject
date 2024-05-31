import "./LivestreamListView.css";
import moment from "moment/moment";
import { Chart } from "react-google-charts";
import {useEffect} from "react";
import SleepAndEmotionalStateBar from "./SleepAndEmotionalStateBar";

const LivestreamStatistics = ({ livestreamStatistics, startedAt }) => {
    useEffect(() => {
        console.log(livestreamStatistics.babyStates)
    }, []);
    const sleepTimePieData = [
        ["Sleep", "Seconds per livestream"],
        ["Awake", livestreamStatistics?.streamDuration - livestreamStatistics?.totalSleepDuration],
        ["Asleep", livestreamStatistics?.totalSleepDuration]
    ]
    const sleepTimePieOptions = {
        title: "Sleep Time",
        is3D: true,
        backgroundColor: "#fcc93b"
    }

    const emotionPieData = [
        ["Emotions", "Emotions frequency per livestream"],
        ...Object.keys(livestreamStatistics?.emotionFrequency).map((key) => {
            return [key, livestreamStatistics?.emotionFrequency[key]];
        })
    ]
    const emotionPieOptions = {
        title: "Emotions Frequency",
        is3D: true,
        backgroundColor: "#fcc93b"
    }

    const mostCommonEmotion = Object.
        keys(livestreamStatistics?.emotionFrequency).
        find(k => livestreamStatistics?.emotionFrequency[k] === Math.
        max(...Object.
        keys(livestreamStatistics?.emotionFrequency).
        map((key) => {
            return livestreamStatistics?.emotionFrequency[key];
        })));

    return (
        <div className={"livestream-statistics"}>
            <div>
                <div className={"sleep-states-container"}>
                    <p>
                        Awake time: {" "}
                        <span className={"sleep-states-value"}>
                            {`${moment.duration(
                                livestreamStatistics.streamDuration - livestreamStatistics.totalSleepDuration, 'seconds').
                            format("h:mm:ss")}`}
                        </span>
                    </p>
                    <p>
                        Sleep time: {" "}
                        <span className={"sleep-states-value"}>
                            {`${moment.duration(
                                livestreamStatistics.totalSleepDuration, 'seconds').
                            format("h:mm:ss")}`}
                        </span>
                    </p>
                </div>
                <Chart
                    chartType="PieChart"
                    data={sleepTimePieData}
                    options={sleepTimePieOptions}
                    width={"17rem"}
                    height={"12rem"}
                />
            </div>

            <div>
                <div className={"sleep-states-container"}>
                    <p>
                        Most common emotion: {" "}
                        <span className={"sleep-states-value"}>
                            {mostCommonEmotion}
                        </span>
                    </p>
                </div>
                <Chart
                    chartType="PieChart"
                    data={emotionPieData}
                    options={emotionPieOptions}
                    width={"17rem"}
                    height={"12rem"}
                />
            </div>
            <div style={{ flex: 1 }}>
                <SleepAndEmotionalStateBar
                    sleepAndEmotionsState={livestreamStatistics?.babyStatesSleepIntervals}
                    totalTime={livestreamStatistics?.streamDuration}
                    startedAt={startedAt}
                />
            </div>
        </div>
    )
}

export default LivestreamStatistics;