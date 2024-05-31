import "./LivestreamListView.css";
import {useEffect, useState} from "react";
import api from "../../utils/api";
import LivestreamListTable from "./LivestreamListTable";

const LivestreamListView = () => {
    const [livestreams, setLivestreams] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const request = async () => {
            try {
                const res = await api.get("livestream/all");

                if (res.status === 200) {
                    console.log(res.data);
                    setLivestreams(res.data);
                }
            } catch (error)
            {
                console.log(error);
            }
        }
        if (loading) {
            request().then(r => {
                setLoading(false);
            });
        }
    }, [loading]);

    return livestreams && livestreams.length !== 0 && (
      <div className={"livestream-list-view"}>
          <div className="monitor-list-view-title-container">
              <h2 className="monitor-list-view-title">Your livestream history:</h2>
              <LivestreamListTable livestreams={livestreams} />
          </div>
      </div>
    );
}

export default LivestreamListView;