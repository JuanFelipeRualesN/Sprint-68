/**
 * Created by portiz on 04/27/2020.
 */
import React, { useState, useEffect } from 'react';
import {Col, Container, Row, Card, CardBody, Table} from 'reactstrap';
import {Link} from "react-router-dom";
import {Link as LinkR} from 'react-scroll'

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import LanguageManager from '../../../custom-classes/language-manager';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faHeart} from '@fortawesome/free-solid-svg-icons';
import ModalVideo from '../../guest-module/modal/modal-video';
import Settings from '../../../custom-classes/settings';
import LiveChat from 'react-livechat';
import SessionManager from "../../../custom-classes/session-manager";
import './style.css';
import offerGnocSIS from '../../../assets/images/common-icons/Landing-icon-Offer.png';
import activityMap from '../../../assets/images/common-icons/Landing-icon-Map.png';
import activityLessonInside from '../../../assets/images/common-icons/Landing-icon-LessonInside.png';
import activityCreateYourAvatar from '../../../assets/images/common-icons/Landing-icon-CreateAvatar.png';
import activityVersus from '../../../assets/images/common-icons/Landing-icon-Versus.png';
import testimonialsImage from '../../../assets/images/common-icons/Landing-icon-Testimonials.jpg';
import activityDashboard from '../../../assets/images/common-icons/Landing-icon-Dashboard.png';
import curriculumImage from '../../../assets/images/common-icons/Landing-icon-Curriculum.png';
import iconEnglish from '../../../assets/images/common-icons/Landing-icon-English.png';
import iconSpanish from '../../../assets/images/common-icons/Landing-icon-Spanish.png';
import iconMath from '../../../assets/images/common-icons/Landing-icon-Math.png';
import mainImage from '../../../assets/images/common-icons/Landing-icon-Main.png';
import activityFunQuizzes from '../../../assets/images/common-icons/Landing-icon-Quizzes.png';
import activityMiniGames from '../../../assets/images/common-icons/Landing-icon-MiniGames.png';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) landing (index page)
 */
const TeachersLanding = () =>  {
    const [chatLiveChatLicense] = useState(Settings.CHAT_LIVE_CHAT_LICENSE);
    const [isMicrosoftTeams] = useState(SessionManager.getIfMicrosoftTeams());
    const [isHovered, setIsHovered] = useState(false);
    const [videoModalOpenend, setVideoModalOpenend] = useState(false);
    const [showOffer, setShowOffer] = useState(true);

    const btnClass = isHovered ? "pulse animated" : "";

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.add('background-alt');
        
        return () => {
            document.body.classList.remove('background-alt');
        };
    }, []);
    
    const handleHover = () => {
        setIsHovered(!isHovered);
    };

    const openModal = () => {
        setVideoModalOpenend(!videoModalOpenend);
    };

    const hideOffer = () => {
        setShowOffer(false);
    };
        return (
            <div>
                <div className="landing-main-teachers animated delay-05s fadeIn" style={{textAlign: "center"}}>
                    <div style={{position: 'absolute', bottom: '120px', left: '50%', transform: 'translateX(-50%)'}}>
                    <h2  >
                        <Translate id="landing-teachers.genial-for-teachers" options={{renderInnerHtml: true}}/>
                    </h2>
                    <hr/>
                    <Link className="rounding-button-aqua btn-default button-secondary-custom" to="/register-free">
                        <Translate id="landing-teachers.register" options={{renderInnerHtml: true}}/>
                    </Link>
                    </div>


                    <div className="courses-title dot-orange-background">
                        <span style={{fontSize:'20px'}}><Translate id="landing-page.subjects-main" options={{renderInnerHtml: true}}/></span>
                        <br/>
                        <span style={{fontSize:'14px'}}><Translate id="landing-page.grades-main" options={{renderInnerHtml: true}}/></span>
                    </div>
                    <br/>
                    {
                        videoModalOpenend ?
                            <ModalVideo visible={videoModalOpenend} updateParent={openModal}/> : null
                    }
                </div>

                <div name="section1" className="container-padding animated delay-05s fadeIn">
                    <Container className="container-wrapper">
                        <Row className="text-center">
                            <Col xs={12}>
                                <Col xs={12}>
                                    <h2>
                                        <Translate id="landing-teachers.educational-program" options={{renderInnerHtml: true}}/>
                                    </h2>
                                </Col>
                            </Col>
                        </Row>
                        <Row className="text-center">
                            <Col lg={{size: 2}} md={{size: 4}} sm={{size: 6}} xs={{size: 6}}>
                                <Card className="card-blue border-none animated delay-05s fadeIn">
                                    <CardBody>
                                        <h2 className="landing-box-font-size">
                                            <Translate id="landing-page.travel-around"
                                                       options={{renderInnerHtml: true}}/></h2>
                                        <img src={activityMap}
                                             className="img-fluid" alt="lessons"/>
                                    </CardBody>
                                </Card>

                            </Col>
                            <Col lg={{size: 2}} md={{size: 4}} sm={{size: 6}} xs={{size: 6}}>

                                <Card className="card-orange border-none animated delay-1s fadeIn">
                                    <CardBody>
                                        <h2 className="landing-box-font-size">
                                            <Translate id="landing-page.lessons-activity"
                                                       options={{renderInnerHtml: true}}/></h2>
                                        <img src={activityLessonInside}
                                             className="img-fluid" alt="lessons"/>
                                    </CardBody>
                                </Card>

                            </Col>

                            <Col lg={{size: 2}} md={{size: 4}} sm={{size: 6}} xs={{size: 6}}>

                                <Card className="card-green border-none animated delay-1-5s fadeIn">
                                    <CardBody>
                                        <h2 className="landing-box-font-size">
                                            <Translate id="landing-page.create-avatar"
                                                       options={{renderInnerHtml: true}}/></h2>
                                        <img src={activityCreateYourAvatar}
                                             className="img-fluid" alt="lessons"/>
                                    </CardBody>
                                </Card>

                            </Col>

                            <Col lg={{size: 2}} md={{size: 4}} sm={{size: 6}} xs={{size: 6}}>

                                <Card className="card-red border-none animated delay-2s fadeIn">
                                    <CardBody>
                                        <h2 className="landing-box-font-size">
                                            <Translate id="landing-page.compete" options={{renderInnerHtml: true}}/>
                                        </h2>
                                        <img src={activityVersus}
                                             className="img-fluid" alt="lessons"/>
                                    </CardBody>
                                </Card>
                            </Col>


                            <Col lg={{size: 2}} md={{size: 4}} sm={{size: 6}} xs={{size: 6}}>

                                <Card className="card-cyan border-none animated delay-2-5s fadeIn">
                                    <CardBody>
                                        <h2 className="landing-box-font-size">
                                            <Translate id="landing-page.mini-games" options={{renderInnerHtml: true}}/>
                                        </h2>
                                        <img src={activityMiniGames}
                                             className="img-fluid" alt="lessons"/>
                                    </CardBody>
                                </Card>
                            </Col>


                            <Col lg={{size: 2}} md={{size: 4}} sm={{size: 6}} xs={{size: 6}}>

                                <Card className="card-violet border-none animated delay-3s fadeIn">
                                    <CardBody>
                                        <h2 className="landing-box-font-size">
                                            <Translate id="landing-page.fun-quizzes" options={{renderInnerHtml: true}}/>
                                        </h2>
                                        <img src={activityFunQuizzes}
                                             className="img-fluid" alt="lessons"/>
                                    </CardBody>
                                </Card>

                            </Col>
                        </Row>
                        <Row>
                            <hr/>
                        </Row>

                        <Row className="text-center">
                            <Col xs={12}>
                                <Card className="card-gray border-none">
                                    <CardBody>
                                        <Row>
                                            <Col lg={{size: 8}} md={{size: 12}} sm={{size: 12}} xs={{size: 12}}>
                                                <div className="font-size-18px text-left">
                                                    <Translate id="landing-page.main-subtitle"
                                                               options={{renderInnerHtml: true}}/>
                                                </div>
                                                <div className="blue-text font-size-18px text-left">
                                                    <LinkR onMouseEnter={handleHover}
                                                           onMouseLeave={handleHover}
                                                           className={btnClass + " btn btn-default button-secondary-custom white-text  btn-bigger"}
                                                           activeClass="active" to="section2" spy={true} smooth={true}
                                                           duration={500}>
                                                        <Translate id="landing-page.what-we-include"/>
                                                    </LinkR>
                                                </div>
                                            </Col>
                                            <Col lg={{size: 4}} md={{size: 0}} sm={{size: 0}} xs={{size: 0}}>
                                                <img src={mainImage} className="img-fluid"
                                                     alt="Main promo"/>
                                            </Col>
                                        </Row>
                                    </CardBody>
                                </Card>
                            </Col>
                            <Col xs={12}>
                                <Table responsive>
                                    <thead className="table-head-blue white-text">
                                    <tr>
                                        <th>
                                            <Translate id="landing-page.subject"/>
                                        </th>
                                        <th>K</th>
                                        <th>1</th>
                                        <th>2</th>
                                        <th>3</th>
                                        <th>4</th>
                                        <th>5</th>
                                        <th>6</th>
                                        <th>7</th>
                                        <th>8</th>
                                        <th>9</th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    <tr>
                                        <th scope="row" className="table-td-main text-right">
                                            <Translate id="landing-page.spanish"/>
                                        </th>
                                        <td/>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                    </tr>
                                    <tr>
                                        <th scope="row" className="table-td-main text-right">
                                            <Translate id="landing-page.english"/>
                                        </th>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                    </tr>
                                    <tr>
                                        <th scope="row" className="table-td-main text-right">
                                            <Translate id="landing-page.math-sp"/>
                                        </th>
                                        <td/>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                        <td className="table-td-fill white-text">&bull;</td>
                                    </tr>
                                    </tbody>
                                </Table>
                            </Col>
                        </Row>
                    </Container>
                </div>

                <div name="section2" className="container-orange container-padding animated delay-05s fadeIn">
                    <Container className="container-wrapper">
                        <Row className="text-center text-white margin-top-mandatory-25">
                            <Col xs={12}>
                                <h1 >
                                    <Translate id="landing-page.subjects"/>
                                </h1>
                                <hr/>
                                <h2><Translate id="landing-page.subjects-include"/></h2>
                            </Col>

                            <Col xs={12}>
                                <Row>
                                    <Col lg={{size: 4}} md={{size: 4}} sm={{size: 12}} xs={{size: 12}}>
                                        <Card className="green-text">
                                            <CardBody>
                                                <img src={iconMath} className="img-fluid"
                                                     alt="Main promo"/>

                                                <hr/>
                                                <h2><Translate id="landing-page.math"/></h2>
                                            </CardBody>
                                        </Card>
                                    </Col>
                                    <Col lg={{size: 4}} md={{size: 4}} sm={{size: 12}} xs={{size: 12}}>
                                        <Card className="blueish-text">
                                            <CardBody>
                                                <img src={iconSpanish} className="img-fluid"
                                                     alt="Main promo"/>
                                                <hr/>
                                                <h2><Translate id="landing-page.spanish"/>
                                                </h2>
                                            </CardBody>
                                        </Card>
                                    </Col>
                                    <Col lg={{size: 4}} md={{size: 4}} sm={{size: 12}} xs={{size: 12}}>
                                        <Card className="violet-text">
                                            <CardBody>
                                                <img src={iconEnglish} className="img-fluid"
                                                     alt="Main promo"/>
                                                <hr/>
                                                <h2><Translate id="landing-page.english"/>
                                                </h2>
                                            </CardBody>
                                        </Card>
                                    </Col>
                                </Row>
                                <hr/>
                                <LinkR onMouseEnter={handleHover} onMouseLeave={handleHover}
                                       activeClass="active" to="section3" spy={true} smooth={true} duration={500}
                                       className={btnClass + " btn-default button-secondary-custom white-text  btn-bigger"}>
                                    <Translate id="landing-page.our-curriculum"/>
                                </LinkR>
                            </Col>
                        </Row>
                    </Container>
                </div>

                <div className="container-blue white-text container-padding animated delay-05s fadeIn" name="section3">
                    <Container className="container-wrapper">
                        <Row className="text-center margin-top-mandatory-50">
                            <Col lg={{size: 10, offset: 1}} md={{size: 10, offset: 1}} sm={{size: 10, offset: 1}}
                                 xs={{size: 10, offset: 1}}>
                                <img src={curriculumImage} className="img-fluid" alt="Main promo"/>
                            </Col>
                            <Col xs={12}>
                                <h1 >
                                    <Translate id="landing-page.curriculum" options={{renderInnerHtml: true}}/>
                                </h1>
                                <hr/>
                                <h2><Translate id="landing-page.curriculum-1" options={{renderInnerHtml: true}}/></h2>
                                <h2><Translate id="landing-page.curriculum-3" options={{renderInnerHtml: true}}/>
                                    <FontAwesomeIcon className="fa-2x red-text" icon={faHeart}/>.</h2>
                                <LinkR onMouseEnter={handleHover} onMouseLeave={handleHover}
                                       activeClass="active" to="section4" spy={true} smooth={true} duration={500}
                                       className={btnClass + " btn btn-default button-secondary-custom white-text  btn-bigger"}>
                                    <Translate id="landing-page.fun-activities"/>
                                </LinkR>
                            </Col>
                        </Row>
                    </Container>
                </div>

                <div className="container-white container-padding animated delay-05s fadeIn" name="section4">
                    <Container className="container-wrapper">
                        <Row className="text-center">
                            <Col xs={12}>
                                <h1 >
                                    <Translate id="landing-page.track-progress" options={{renderInnerHtml: true}}/>
                                </h1>
                                <hr/>
                                <img src={activityDashboard} className="img-fluid"
                                     alt="Landing Math"/>
                                <h2>
                                    <Translate id="landing-page.track-progress-1" options={{renderInnerHtml: true}}/>
                                </h2>
                            </Col>
                        </Row>
                    </Container>
                </div>

                <div className="container-teal container-padding white-text"
                     style={{backgroundImage: "url(" + testimonialsImage + ")"}}>
                    <Container className="container-wrapper">
                        <Row className="text-center">
                            <Col xs={12}>
                                <h1 >
                                    <Translate id="landing-page.testimonials" options={{renderInnerHtml: true}}/>
                                </h1>
                                <hr/>
                                <h2>
                                    <Translate id="landing-page.testimonial-1" options={{renderInnerHtml: true}}/>
                                </h2>
                                <br/>
                                <h2>
                                    <Translate id="landing-page.testimonial-2" options={{renderInnerHtml: true}}/>
                                </h2>
                                <br/>
                                <h2>
                                    <Translate id="landing-page.testimonial-3" options={{renderInnerHtml: true}}/>
                                </h2>
                            </Col>
                        </Row>
                    </Container>
                </div>

                <div className="container-white container-padding">
                    <Container className="container-wrapper">
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                                <img src={offerGnocSIS}
                                     className="img-fluid animated pulse infinite"
                                     alt="Offer"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                                <Card className="card-red">
                                    <CardBody>
                                        <h1 className=" centered-text">
                                            <Translate id="landing-teachers.register-now" options={{renderInnerHtml: true}}/>
                                        </h1>
                                        <hr/>
                                        <h2 className=" centered-text">
                                            <Translate id="landing-teachers.notes"
                                                       options={{renderInnerHtml: true}}/>
                                        </h2>
                                        <p><Translate id="landing-teachers.note-explanation"
                                                      options={{renderInnerHtml: true}}/></p>
                                        <div className="centered-text">
                                            <Link className="rounding-button-aqua btn-default button-secondary-custom" to="/register-free">
                                                <Translate id="landing-teachers.register" options={{renderInnerHtml: true}}/>
                                            </Link>
                                        </div>
                                    </CardBody>
                                </Card>
                            </Col>
                        </Row>

                    </Container>
                    {!isMicrosoftTeams ?
                        <LiveChat license={chatLiveChatLicense}/>
                        : null
                    }
                </div>

            </div>
        )
    };


export default withLocalize(TeachersLanding);