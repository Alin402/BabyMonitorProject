import "./TemperatureData.css";

const TemperatureData = ( {temperatureData} ) => {
    return (
        <div className="temperature-data">
           <div className="temperature_data_right temperature-data-section">
                {
                    temperatureData.temperatureC.toFixed(2) 
                }
                &#176;
                C
           </div>
           <div className="temperature_data_middle temperature-data-section">
                {
                    temperatureData.temperatureF.toFixed(2)
                }
                &#176;
                F
           </div>
           <div className="temperature_data_left temperature-data-section">
                {
                    temperatureData.humidity.toFixed(2)
                }
                %
           </div>
        </div>
    );
}

export default TemperatureData;
