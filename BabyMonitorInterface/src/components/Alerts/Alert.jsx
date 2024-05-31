import "./Alert.css";
import CloseIcon from '@mui/icons-material/Close';


const Alert = ({ Type, msg, setClose, children}) => {
    let typeToColor = {
        "success": "#269f67",
        "error": "#c8224d",
        "info": "#fcc93b"
    }
    return (
    <div className="alert-container">
        <div className="alert" style={{ backgroundColor: typeToColor[Type] || "black" }}>
            <div className="close-icon" onClick={ () => setClose(false) }>
                <CloseIcon sx={{ color: "#272c2b" }} />
            </div>
            <h2>
                {msg}
            </h2>
            {
                children
            }
        </div>
    </div>
    );
}

export default Alert;