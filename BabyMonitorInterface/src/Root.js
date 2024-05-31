import "./App.css";
import NavigationDrawer from "./components/Navigation/NavigationDrawer/NavigationDrawer";
import { Outlet } from "react-router-dom";
import {LocalizationProvider} from "@mui/x-date-pickers";
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs/AdapterDayjs';
function Root() {

  return (
      <LocalizationProvider dateAdapter={AdapterDayjs}>
          <div style={{ backgroundColor: "#272c2b", height: "100vh" }}>
              <NavigationDrawer />
              <Outlet />
          </div>
      </LocalizationProvider>
  )
}

export default Root;