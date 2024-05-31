import "./LivestreamPlayer.css";
import PlayerComponent from "./PlayerComponent";

function LivestreamPlayer({ playerRef, monitoringDeviceActive, livestreamStreaming, boundingBoxRef, device }) {
    return (
        <div className="player-container">
			<div className="bounding-box" ref={boundingBoxRef}></div>
            <PlayerComponent
                playerRef={playerRef}
                streamId={device.streamId}
            />
        </div>
     );
}

export default LivestreamPlayer;