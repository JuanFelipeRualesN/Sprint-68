import React, { useState } from "react";
import Plyr from "react-plyr";
import "plyr/dist/plyr.css";
import { Modal, ModalBody } from "reactstrap";
import { getAsset } from "../utils/getAssets";

const IconPlaceholder = getAsset('Avanza-icon-Placeholder');

const VideoPlayer = ({ src }) => {
    const [open, setOpen] = useState(false);

    return (
        <>
            <div
                className="video-container cursor-pointer"
                style={{ borderRadius: "15px" }}
                onClick={() => setOpen(true)}
            >
                <img src={IconPlaceholder} className="video-container" alt="video" style={{ width: "100%", borderRadius: "15px" }} />

            </div>
            <Modal isOpen={open} toggle={() => setOpen(!open)} size="lg" centered>
                <ModalBody className="p-0 bg-black" style={{backgroundColor: "black"}}>
                    <Plyr type="video" url={src}  />
                </ModalBody>
            </Modal>
        </>
    );
};

export default VideoPlayer;