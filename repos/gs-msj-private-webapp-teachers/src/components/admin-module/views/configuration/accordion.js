import React, { useState, useEffect, useContext } from "react";
import Accordion from "@material-ui/core/Accordion";
import AccordionSummary from "@material-ui/core/AccordionSummary";
import AccordionDetails from "@material-ui/core/AccordionDetails";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import FormGroup from '@material-ui/core/FormGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import { withStyles } from '@material-ui/core/styles';
import Switch from '@material-ui/core/Switch';
import { Translate } from "react-localize-redux";
import { modifySettingsPowerDe, getSettingsPowerDe } from "../../../../api/gsweb";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import { AppContext } from "../../../../App.Context";
import { appReducerCases } from "../../../../types";

const IOSSwitch = withStyles((theme) => ({
    root: {
        width: 42,
        height: 26,
        padding: 0,
        margin: theme.spacing(1),
    },
    switchBase: {
        padding: 1,
        '&$checked': {
            transform: 'translateX(16px)',
            color: theme.palette.common.white,
            '& + $track': {
                backgroundColor: '#52d869',
                opacity: 1,
                border: 'none',
            },
        },
        '&$focusVisible $thumb': {
            color: '#52d869',
            border: '6px solid #fff',
        },
    },
    thumb: {
        width: 24,
        height: 24,
    },
    track: {
        borderRadius: 26 / 2,
        border: `1px solid ${theme.palette.grey[400]}`,
        backgroundColor: '#989898',
        opacity: 1,
        transition: theme.transitions.create(['background-color', 'border']),
    },
    checked: {},
    focusVisible: {},
}))(({ classes, ...props }) => {
    return (
        <Switch
            focusVisibleClassName={classes.focusVisible}
            disableRipple
            classes={{
                root: classes.root,
                switchBase: classes.switchBase,
                thumb: classes.thumb,
                track: classes.track,
                checked: classes.checked,
            }}
            {...props}
        />
    );
});


const AccordionDetail = () => {
    const { state } = useContext(AppContext);
    const [data, setData] = useState(state.variables);
    const [loading, setLoading] = useState(false);
    const { dispatch } = useContext(AppContext);

    useEffect(() => {
        getParametersPDE();
    }, []);

    const getParametersPDE = async () => {
        setLoading(true);
        try {
            const response = await getSettingsPowerDe();
            const data = response.data;
            const { Valid, Messages, Id, ...settings } = data;
            dispatch({
                type: appReducerCases.setParameters,
                payload: { Id, ...settings },
            });
            setData({ Id, ...settings });
            setLoading(false);
        } catch (error) {
            setLoading(false);
        }
    };

    const handleChange = (event) => {
        setData({ ...data, [event.target.name]: event.target.checked });
        updateDetails({ ...data, [event.target.name]: event.target.checked });
    };

    const handleChangeMessageAdmin = (event) => {
        const checked = event.target.checked;
        setData({ ...data, MessageManagementAdmin: checked, MessageManagementSendAdmin: checked });
        updateDetails({ ...data, MessageManagementAdmin: checked, MessageManagementSendAdmin: checked });
    };

    const handleChangeMessageManagementSendAdmin = (event) => {
        const checked = event.target.checked;
        if (data.MessageManagementAdmin) {
            setData({ ...data, MessageManagementSendAdmin: checked });
            updateDetails({ ...data, MessageManagementSendAdmin: checked });
        } else {
            setData({ ...data, MessageManagementAdmin: checked, MessageManagementSendAdmin: checked });
            updateDetails({ ...data, MessageManagementAdmin: checked, MessageManagementSendAdmin: checked });
        }
    };

    const handleChangeMessageAdminOre = (event) => {
        const checked = event.target.checked;
        setData({ ...data, MessageManagementAdminOre: checked, MessageManagementSendAdminOre: checked });
        updateDetails({ ...data, MessageManagementAdminOre: checked, MessageManagementSendAdminOre: checked });
    };

    const handleChangeMessageManagementSendAdminOre = (event) => {
        const checked = event.target.checked;
        if (data.MessageManagementAdminOre) {
            setData({ ...data, MessageManagementSendAdminOre: checked });
            updateDetails({ ...data, MessageManagementSendAdminOre: checked });
        } else {
            setData({ ...data, MessageManagementAdminOre: checked, MessageManagementSendAdminOre: checked });
            updateDetails({ ...data, MessageManagementAdminOre: checked, MessageManagementSendAdminOre: checked });
        }
    };

    const handleChangeMessageDirector = (event) => {
        const checked = event.target.checked;
        setData({ ...data, MessageManagementPrincipal: checked, MessageManagementSendPrincipal: checked });
        updateDetails({ ...data, MessageManagementPrincipal: checked, MessageManagementSendPrincipal: checked });
    };

    const handleChangeMessageManagementSendPrincipal = (event) => {
        const checked = event.target.checked;
        if (data.MessageManagementPrincipal) {
            setData({ ...data, MessageManagementSendPrincipal: checked });
            updateDetails({ ...data, MessageManagementSendPrincipal: checked });
        } else {
            setData({ ...data, MessageManagementPrincipal: checked, MessageManagementSendPrincipal: checked });
            updateDetails({ ...data, MessageManagementPrincipal: checked, MessageManagementSendPrincipal: checked });
        }
    };

    const handleChangeMessageTeacher = (event) => {
        const checked = event.target.checked;
        setData({ ...data, MessageManagementTeacher: checked, MessageManagementSendTeacher: checked });
        updateDetails({ ...data, MessageManagementTeacher: checked, MessageManagementSendTeacher: checked });
    };

    const handleChangeMessageManagementSendTeacher = (event) => {
        const checked = event.target.checked;
        if (data.MessageManagementTeacher) {
            setData({ ...data, MessageManagementSendTeacher: checked });
            updateDetails({ ...data, MessageManagementSendTeacher: checked });
        } else {
            setData({ ...data, MessageManagementTeacher: checked, MessageManagementSendTeacher: checked });
            updateDetails({ ...data, MessageManagementTeacher: checked, MessageManagementSendTeacher: checked });
        }
    };

    const handleChangeMessageStudent = (event) => {
        const checked = event.target.checked;
        setData({ ...data, MessageManagementStudent: checked, MessageManagementSendStudent: checked });
        updateDetails({ ...data, MessageManagementStudent: checked, MessageManagementSendStudent: checked });
    };

    const handleChangeMessageManagementSendStudent = (event) => {
        const checked = event.target.checked;
        if (data.MessageManagementStudent) {
            setData({ ...data, MessageManagementSendStudent: checked });
            updateDetails({ ...data, MessageManagementSendStudent: checked });
        } else {
            setData({ ...data, MessageManagementStudent: checked, MessageManagementSendStudent: checked });
            updateDetails({ ...data, MessageManagementStudent: checked, MessageManagementSendStudent: checked });
        }
    };

    const handleChangeInvitationTeacher = (event) => {
        const checked = event.target.checked;
        if (checked) {
            setData({ ...data, InvitationManagementTeacher: checked });
            updateDetails({ ...data, InvitationManagementTeacher: checked });
        } else {
            setData({ ...data, InvitationManagementTeacher: checked, InvitationTeacherToStudentManagementTeacher: checked, UserManagementTeacher: checked });
            updateDetails({ ...data, InvitationManagementTeacher: checked, InvitationTeacherToStudentManagementTeacher: checked, UserManagementTeacher: checked });
        }
    };

    const handleChangeInvitationToStudent = (event) => {
        const checked = event.target.checked;
        if (data.InvitationManagementTeacher) {
            setData({ ...data, InvitationTeacherToStudentManagementTeacher: checked });
            updateDetails({ ...data, InvitationTeacherToStudentManagementTeacher: checked });
        } else {
            setData({ ...data, InvitationManagementTeacher: checked, InvitationTeacherToStudentManagementTeacher: checked });
            updateDetails({ ...data, InvitationManagementTeacher: checked, InvitationTeacherToStudentManagementTeacher: checked });
        }
    };

    const handleChangeConfigurationDirector = (event) => {
        const checked = event.target.checked;
        setData({ ...data, ConfigurationManagerPrincipal: checked, PasswordManagementPrincipal: checked });
        updateDetails({ ...data, ConfigurationManagerPrincipal: checked, PasswordManagementPrincipal: checked });
    };

    const handleChangeConfigurationAndPasswordManagementTeacher = (event) => {
        const checked = event.target.checked;
        setData({ ...data, ConfigurationManagerTeacher: checked, PasswordManagementTeacher: checked });
        updateDetails({ ...data, ConfigurationManagerTeacher: checked, PasswordManagementTeacher: checked });
    };

    const handleChangeConfigurationAdminOre = (event) => {
        const checked = event.target.checked;
        setData({ ...data, ConfigurationManagerAdminOre: checked, PasswordManagementAdminOre: checked });
        updateDetails({ ...data, ConfigurationManagerAdminOre: checked, PasswordManagementAdminOre: checked });
    };

    const handleChangeInternalLogin = (event) => {
        const checked = event.target.checked;
        if (!checked && !data.MicrosoftSSOManagement) {
            setData({ ...data, MicrosoftSSOManagement: true, InternalLoginManagement: checked, InternalPasswordReset: checked });
            updateDetails({ ...data, MicrosoftSSOManagement: true, InternalLoginManagement: checked, InternalPasswordReset: checked });
        } else {
            setData({ ...data, InternalLoginManagement: checked, InternalPasswordReset: checked });
            updateDetails({ ...data, InternalLoginManagement: checked, InternalPasswordReset: checked });
        }
    };

    const handleChangeSSO = (event) => {
        const checked = event.target.checked;
        if (!checked && !data.InternalLoginManagement) {
            setData({ ...data, MicrosoftSSOManagement: true });
            updateDetails({ ...data, MicrosoftSSOManagement: true });
        } else {
            setData({ ...data, MicrosoftSSOManagement: checked });
            updateDetails({ ...data, MicrosoftSSOManagement: checked });
        }
    };

    const handleChangeInternalPasswordReset = (event) => {
        const checked = event.target.checked;
        if (checked) {
            setData({ ...data, InternalLoginManagement: checked, InternalPasswordReset: checked });
            updateDetails({ ...data, InternalLoginManagement: checked, InternalPasswordReset: checked });
        } else {
            setData({ ...data, InternalPasswordReset: checked });
            updateDetails({ ...data, InternalPasswordReset: checked });
        }
    };

    const updateDetails = async (data) => {
        setLoading(true);
        try {
            const response = await modifySettingsPowerDe(data);
            const dataMap = response.data;
            const { Valid, Messages, Id, ...settings } = dataMap;
            setData({ Id, ...settings });
            dispatch({
                type: appReducerCases.setParameters,
                payload: { Id, ...settings },
            });
            setLoading(false);
        } catch (error) {
            setLoading(false);
        }
    };

    return (
        <>
            <div className="wrapper-lesson-tabs wrapper-accordion">
                <div className="container_two-col">
                    <div className="wrapper-student-progress accordion-style">
                        <Accordion>
                            <AccordionSummary
                                expandIcon={<ExpandMoreIcon className="icon-color" />}
                            >
                                <p><Translate id="module-settings.role-admin"
                                              options={{ renderInnerHtml: false }} /></p>
                            </AccordionSummary>
                            <AccordionDetails>
                                <div>
                                    <small>
                                        <Translate id="module-settings.subtitle"
                                                   options={{ renderInnerHtml: false }} />
                                    </small>
                                    <FormGroup>
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.SchoolManagementAdmin} onChange={handleChange} name="SchoolManagementAdmin" />}
                                            label={<Translate id="module-settings.management-school" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.EnrollmentManagementAdmin} onChange={handleChange} name="EnrollmentManagementAdmin" />}
                                            label={<Translate id="module-settings.management-enrollment" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.UserManagementAdmin} onChange={handleChange} name="UserManagementAdmin" />}
                                            label={<Translate id="module-settings.management-user" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementAdmin} onChange={handleChangeMessageAdmin} name="MessageManagementAdmin" />}
                                            label={<Translate id="module-settings.module-message" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementSendAdmin} onChange={handleChangeMessageManagementSendAdmin} name="MessageManagementSendAdmin" />}
                                            label={<Translate id="module-settings.message-send" />}
                                        />
                                        {/* <FormControlLabel
                                            control={<IOSSwitch checked={data.ConfigurationManagerAdmin} onChange={handleChange} name="ConfigurationManagerAdmin" />}
                                            label={<Translate id="module-settings.settings" />}
                                        /> */}
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.PasswordManagementAdmin} onChange={handleChange} name="PasswordManagementAdmin" />}
                                            label={<Translate id="module-settings.management-password" />}
                                        />
                                    </FormGroup>
                                </div>
                            </AccordionDetails>
                        </Accordion>
                        <Accordion className="mt-3">
                            <AccordionSummary
                                expandIcon={<ExpandMoreIcon className="icon-color" />}
                            >
                                <p><Translate id="module-settings.role-director"
                                              options={{ renderInnerHtml: false }} /></p>
                            </AccordionSummary>
                            <AccordionDetails>
                                <di>
                                    <small>
                                        <Translate id="module-settings.subtitle"
                                                   options={{ renderInnerHtml: false }} />
                                    </small>
                                    <FormGroup>
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.UserManagementPrincipal} onChange={handleChange} name="UserManagementPrincipal" />}
                                            label={<Translate id="module-settings.management-user" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementPrincipal} onChange={handleChangeMessageDirector} name="MessageManagementPrincipal" />}
                                            label={<Translate id="module-settings.module-message" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementSendPrincipal} onChange={handleChangeMessageManagementSendPrincipal} name="MessageManagementSendPrincipal" />}
                                            label={<Translate id="module-settings.message-send" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.ConfigurationManagerPrincipal} onChange={handleChangeConfigurationDirector} name="ConfigurationManagerPrincipal" />}
                                            label={<Translate id="module-settings.settings" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.PasswordManagementPrincipal} onChange={handleChangeConfigurationDirector} name="PasswordManagementPrincipal" />}
                                            label={<Translate id="module-settings.management-password" />}
                                        />
                                    </FormGroup>
                                </di>
                            </AccordionDetails>
                        </Accordion>
                        <Accordion className="mt-3">
                            <AccordionSummary
                                expandIcon={<ExpandMoreIcon className="icon-color" />}
                            >
                                <p><Translate id="module-settings.role-student"
                                              options={{ renderInnerHtml: false }} /></p>
                            </AccordionSummary>
                            <AccordionDetails>
                                <div>
                                    <small>
                                        <Translate id="module-settings.subtitle"
                                                   options={{ renderInnerHtml: false }} />
                                    </small>
                                    <FormGroup>
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementStudent} onChange={handleChangeMessageStudent} name="MessageManagementStudent" />}
                                            label={<Translate id="module-settings.module-message" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementSendStudent} onChange={handleChangeMessageManagementSendStudent} name="MessageManagementSendStudent" />}
                                            label={<Translate id="module-settings.message-send" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.ProgressManagementStudent} onChange={handleChange} name="ProgressManagementStudent" />}
                                            label={<Translate id="module-settings.progress-module" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.TeamsManagementStudent} onChange={handleChange} name="TeamsManagementStudent" />}
                                            label={<Translate id="module-settings.teams-module" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.ChatManagementStudent} onChange={handleChange} name="ChatManagementStudent" />}
                                            label={<Translate id="module-settings.chat-module" />}
                                        />
                                    </FormGroup>
                                </div>
                            </AccordionDetails>
                        </Accordion>
                        <Accordion className="mt-3">
                            <AccordionSummary
                                expandIcon={<ExpandMoreIcon className="icon-color" />}
                            >
                                <p><Translate id="module-settings.sections"
                                              options={{ renderInnerHtml: false }} /></p>
                            </AccordionSummary>
                            <AccordionDetails>
                                <div>
                                    <small>
                                        <Translate id="module-settings.subtitle"
                                                   options={{ renderInnerHtml: false }} />
                                    </small>
                                    <FormGroup>
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MicrosoftSSOManagement} onChange={handleChangeSSO} name="MicrosoftSSOManagement" />}
                                            label={<Translate id="module-settings.sso" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.InternalLoginManagement} onChange={handleChangeInternalLogin} name="InternalLoginManagement" />}
                                            label={<Translate id="module-settings.internal-login" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.InternalPasswordReset} onChange={handleChangeInternalPasswordReset} name="InternalPasswordReset" />}
                                            label={<Translate id="module-settings.recover-pass" />}
                                        />
                                    </FormGroup>
                                </div>
                            </AccordionDetails>
                        </Accordion>
                    </div>
                </div>

                <div className="container_two-col">
                    <div className="wrapper-student-progress accordion-style">
                        <Accordion>
                            <AccordionSummary
                                expandIcon={<ExpandMoreIcon className="icon-color" />}
                            >
                                <p><Translate id="module-settings.role-admin-ore"
                                              options={{ renderInnerHtml: false }} /></p>
                            </AccordionSummary>
                            <AccordionDetails>
                                <div>
                                    <small>
                                        <Translate id="module-settings.subtitle"
                                                   options={{ renderInnerHtml: false }} />
                                    </small>
                                    <FormGroup>
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.SchoolManagementAdminOre} onChange={handleChange} name="SchoolManagementAdminOre" />}
                                            label={<Translate id="module-settings.management-school" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.EnrollmentManagementAdminOre} onChange={handleChange} name="EnrollmentManagementAdminOre" />}
                                            label={<Translate id="module-settings.management-enrollment" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.UserManagementAdminOre} onChange={handleChange} name="UserManagementAdminOre" />}
                                            label={<Translate id="module-settings.management-user" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementAdminOre} onChange={handleChangeMessageAdminOre} name="MessageManagementAdminOre" />}
                                            label={<Translate id="module-settings.module-message" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementSendAdminOre} onChange={handleChangeMessageManagementSendAdminOre} name="MessageManagementSendAdminOre" />}
                                            label={<Translate id="module-settings.message-send" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.ConfigurationManagerAdminOre} onChange={handleChangeConfigurationAdminOre} name="ConfigurationManagerAdminOre" />}
                                            label={<Translate id="module-settings.settings" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.PasswordManagementAdminOre} onChange={handleChangeConfigurationAdminOre} name="PasswordManagementAdminOre" />}
                                            label={<Translate id="module-settings.management-password" />}
                                        />
                                    </FormGroup>
                                </div>
                            </AccordionDetails>
                        </Accordion>
                        <Accordion className="mt-3">
                            <AccordionSummary
                                expandIcon={<ExpandMoreIcon className="icon-color" />}
                            >
                                <p><Translate id="module-settings.role-teacher"
                                              options={{ renderInnerHtml: false }} /></p>
                            </AccordionSummary>
                            <AccordionDetails>
                                <div>
                                    <small>
                                        <Translate id="module-settings.subtitle"
                                                   options={{ renderInnerHtml: false }} />
                                    </small>
                                    <FormGroup>
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.InvitationManagementTeacher} onChange={handleChangeInvitationTeacher} name="InvitationManagementTeacher" />}
                                            label={<Translate id="module-settings.invitations-module" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.InvitationTeacherToStudentManagementTeacher} onChange={handleChangeInvitationToStudent} name="InvitationTeacherToStudentManagementTeacher" />}
                                            label={<Translate id="module-settings.student-invitations" />}
                                        />
                                        {/* <FormControlLabel
                                            control={<IOSSwitch checked={data.InvitationTeacherToParentManagementTeacher} onChange={handleChange} name="InvitationTeacherToParentManagementTeacher" />}
                                            label={<Translate id="module-settings.parent-invitations" />}
                                        /> */}
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.UserManagementTeacher} onChange={handleChange} name="UserManagementTeacher" />}
                                            label={<Translate id="module-settings.management-user" />}
                                        />
                                        {/* <FormControlLabel
                                            control={<IOSSwitch checked={data.GroupInternalManagementTeacher} onChange={handleChange} name="GroupInternalManagementTeacher" />}
                                            label={<Translate id="module-settings.apply-subgroups" />}
                                        /> */}
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.ChatManagementTeacher} onChange={handleChange} name="ChatManagementTeacher" />}
                                            label={<Translate id="module-settings.chat-module" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementTeacher} onChange={handleChangeMessageTeacher} name="MessageManagementTeacher" />}
                                            label={<Translate id="module-settings.module-message" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.MessageManagementSendTeacher} onChange={handleChangeMessageManagementSendTeacher} name="MessageManagementSendTeacher" />}
                                            label={<Translate id="module-settings.message-send" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.TeamMeetsManagementTeacher} onChange={handleChange} name="TeamMeetsManagementTeacher" />}
                                            label={<Translate id="module-settings.teams-module" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.ConfigurationManagerTeacher} onChange={handleChangeConfigurationAndPasswordManagementTeacher} name="ConfigurationManagerTeacher" />}
                                            label={<Translate id="module-settings.settings" />}
                                        />
                                        <FormControlLabel
                                            control={<IOSSwitch checked={data.PasswordManagementTeacher} onChange={handleChangeConfigurationAndPasswordManagementTeacher} name="PasswordManagementTeacher" />}
                                            label={<Translate id="module-settings.management-password" />}
                                        />
                                    </FormGroup>
                                </div>
                            </AccordionDetails>
                        </Accordion>
                    </div>
                </div>
            </div>
            {loading ? <LoadingFullWindow /> : null}
        </>
    );
};

export default AccordionDetail;
