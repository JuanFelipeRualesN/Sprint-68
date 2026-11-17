import React, { useState, useEffect, useContext } from "react";
import ProfileUpdate from "./profile-update";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { SendAccessLog } from "../../../utils/access_logger";


const ProfileUpdateTeacher = () => {
    const sessionDetails = SessionManager.getSessionDetails();

    useEffect(() => {
        try{            
        SendAccessLog({
            Username: sessionDetails.Username,
            InvitationCode: sessionDetails.InvitationCode,
            Role: sessionDetails.Roles[0],
            AreaId: "19"
        })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);


    return (
        <ProfileUpdate roleId={Settings.ROLE_TEACHER_ID}
        />
    );
};

export default ProfileUpdateTeacher;
