import { Button, Divider } from "@mui/material";
import ContactSupportIcon from '@mui/icons-material/ContactSupport';
import Alert from "../Alert";

const ChoiceAlert = ({ setClose, msg, handleYes }) => {
    return (
        <Alert
            Type="info"
            setClose={setClose}
        >
            <div style={{display: "flex", color: "#272c2b"}}>
                <div>
                    <h2>{msg}</h2>
                    <Divider/>
                    <div style={{marginTop: "2rem"}}>
                        <Button style={{borderColor: "#272c2b", color: "#272c2b"}} variant="outlined"
                                onClick={handleYes}>Yes</Button>
                        <Button style={{marginLeft: "10px", borderColor: "#272c2b", color: "#272c2b"}}
                                variant="outlined" onClick={() => setClose(false)}>No</Button>
                    </div>
                </div>
                <div style={{padding: "1rem"}}>
                    <ContactSupportIcon sx={{fontSize: "5em"}}/>
                </div>
            </div>
        </Alert>
    )
}

export default ChoiceAlert;