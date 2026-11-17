import React, {useEffect} from "react";
import ProfileUpdate from "./profile-update";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { SendAccessLog } from "../../../utils/access_logger";


const ProfileUpdateStudent = () => {
    const userDetails = SessionManager.getSessionDetails();

    useEffect(() => {
        try{            
            SendAccessLog({
                UserName: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "1",
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }

    }, []);
    return (
        <ProfileUpdate roleId={Settings.ROLE_STUDENT_ID}
                       withPromptAlerts
        />
    );
};

export default ProfileUpdateStudent;
