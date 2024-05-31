const PlayerComponent = ({ playerRef, streamId }) => {
    return (
        <iframe
            src={"https://lvpr.tv?v=" + streamId}
            allowFullScreen
            allow="autoplay; encrypted-media; fullscreen; picture-in-picture"
            frameBorder="0"
            style={{ width: "100%", height: "100%" }}
            ref={playerRef}
        >
        </iframe>
    )
}

export default PlayerComponent;