import React, { useState, useEffect } from "react";
import Dialog from "@material-ui/core/Dialog";
import DialogContent from "@material-ui/core/DialogContent";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";


const AddPostDialog = ({
                           onClose,
                           open,
                           forumId,
                           getForumDetailById,
                           item,
                       }) => {
    const { t } = useTranslation(["forum"]);
    const [fullWidth] = useState(true);
    const [maxWidth] = useState("md");
    const [description, setDescription] = useState("");
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (item.Id) {
            setValues();
        }
    }, []);

    const setValues = () => {
        setDescription(item.Body);
    };

    const handleClose = () => {
        onClose();
    };

    const handleSubmit = () => {
        if (item.Id) {
            editPost();
        } else {
            addPost();
        }
    };

    const addPost = async () => {
        const req = {
            ForumId: forumId,
            Body: description,
        };
        setLoading(true);
        try {
            await createPost(req);
            getForumDetailById();
            setLoading(false);
            handleClose();
        } catch {
            setLoading(false);
        }
    };

    const editPost = async () => {
        const req = {
            Id: item.Id,
            Body: description,
        };
        setLoading(true);
        try {
            await updatePost(req);
            getForumDetailById();
            setLoading(false);
            handleClose();
        } catch {
            setLoading(false);
        }
    };

    return (
        <>
            <Dialog
                onClose={handleClose}
                open={open}
                fullWidth={fullWidth}
                maxWidth={maxWidth}
            >
                <DialogContent>
                    <div className="modal-text-right">
                        <button className="close-windows" onClick={handleClose}>
                            <i className="fas fa-times"></i>
                        </button>
                    </div>

                    <h1 className="title-modal-message title-modal-message__forum">
                        {item.Id ? t("editPost") : t("newPost")}
                    </h1>

                    <div className="modal-message--compose-body body-forum">
                        <div className="form-group-message-compose">
              <textarea
                  id="message"
                  placeholder={t("placeText")}
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
              ></textarea>
                        </div>

                        <button
                            type="submit"
                            className="button-modal-message-primary"
                            disabled={!description}
                            onClick={handleSubmit}
                        >
                            {t("send")}
                        </button>
                    </div>
                    {loading && <LoadingFullWindow />}
                </DialogContent>
            </Dialog>
        </>
    );
};

export default AddPostDialog;
