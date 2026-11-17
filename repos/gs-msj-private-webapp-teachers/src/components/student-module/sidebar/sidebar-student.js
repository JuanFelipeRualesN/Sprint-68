/**
 * Created by portiz on 09/03/20.
 */
import React from 'react';
// Be sure to include styles at some point, probably during your bootstraping
import '@trendmicro/react-sidenav/dist/react-sidenav.css';
import {Col, Row} from "reactstrap";
import {Translate} from "react-localize-redux";
import {Link} from "react-router-dom";
import './style.css';
import ImgHomeIcon from "../../../assets/images/home-icon.png";
import ImgMainIcon from "../../../assets/images/main-icon.png";

const SidebarStudent = ({ selected }) => {

    return (
        <Col xs={2} md={2} lg={1} className="sidebar-background sidebar-padding-top">
            <Row className="text-center white-text">
                <Col xs={12} className={selected === '1' ? 'btn-transparent-selected-no-radius': ''}>
                    <Link to="/dashboard" className="link-white">
                    <img src={ImgHomeIcon}
                         alt="Home"
                         className="sidenav-icons"/>
                <br/>
                    <small> <Translate id="sidebar-students.home"/></small>
                    </Link>
                </Col>
                <Col xs={12} className={selected === '2' ? 'btn-transparent-selected-no-radius margin-top-mandatory-25': 'margin-top-mandatory-25'}>
                    <Link to="/messages-student" className="link-white">
                    <img src={ImgMainIcon}
                         alt="Home"
                         className="sidenav-icons"/>
                    <br/>
                    <small> <Translate id="sidebar-students.messages"/></small>
                    </Link>
                </Col>
            </Row>
        </Col>
    );
};

export default SidebarStudent;