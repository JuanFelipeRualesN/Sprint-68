import React from "react";
import "./maintenance.css";
import LogoImg from "../../../assets/images/common-icons/Common-icon-maintenanceLogo.svg";
import mantenimientoImage from '../../../assets/images/common-icons/Common-icon-maintenance.png';

const Maintenance = () => {
  return (
    <>
      <div className="bg-mantenimiento">
        <div id="page">
          <div id="content">

            <div className="full-col">
              <div className="full-col__mod-txt">
                <img className="full-col__mod-txt-logo" src={LogoImg} />
                <h1 className="color-500">Volvemos enseguida</h1>
                <p className="color-white">Estamos realizando tareas de mantenimiento, disculpe las molestias. Estaremos de vuelta en breve.</p>
                <img src={mantenimientoImage} />
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Maintenance;
