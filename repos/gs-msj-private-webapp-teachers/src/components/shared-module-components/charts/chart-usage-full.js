/**
 * Created by portiz on 2/4/20.
 */
import React, { useState, useEffect } from 'react';
import {
    Col,
    Row,
Table,
Media
} from 'reactstrap';
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";

/** End of Multiple Language imports **/
import LanguageManager from '../../../custom-classes/language-manager';
import PieChart from './chart-pie-full';
import ChartBar from './chart-bar-2';
import LegendCanvas from '../canvas/legend-canvas';
import Settings from "../../../custom-classes/settings";
import logo from '../../../assets/images/logo.png';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle the user details
 */
const ChartReportUsage = (props) => {
    const [activeUsersTRs, setActiveUsersTRs] = useState(null);
    const [dateDetails, setDateDetails] = useState(props.dateDetails ?? null);
    const [dateFrom, setDateFrom] = useState(props.dateFrom ?? null);
    const [dateTo, setDateTo] = useState(props.dateTo ?? null);
    const [dayChartShow, setDayChartShow] = useState(props.dayChartShow ?? false);
    const [didMount, setDidMount] = useState(false);
    const [entityNames, setEntityNames] = useState(props.entityNames ?? null);
    const [inactiveUsersTRs, setInactiveUsersTRs] = useState(null);
    const [languages] = useState(Settings.LANGUAGES);
    const [loading, setLoading] = useState(false);
    const [monthDetails, setMonthDetails] = useState(props.monthDetails ?? null);
    const [monthlyChartShow, setMonthlyChartShow] = useState(props.monthlyChartShow ?? false);
    const [profilesActive, setProfilesActive] = useState(props.profilesActive ?? null);
    const [profilesActiveNonActiveChartData, setProfilesActiveNonActiveChartData] = useState(null);
    const [profilesDetails, setProfilesDetails] = useState(props.profilesDetails ?? null);
    const [profilesNonActive, setProfilesNonActive] = useState(props.profilesNonActive ?? null);
    const [profilesQuantity, setProfilesQuantity] = useState(props.profilesQuantity ?? null);
    const [propsModal, setPropsModal] = useState({ bodyText: '', visible: false });
    const [subscriptionsDetails, setSubscriptionsDetails] = useState(props.subscriptionsDetails ?? null);
    const [yearDetails, setYearDetails] = useState(props.yearDetails ?? null);
    const [yearlyChartShow, setYearlyChartShow] = useState(props.yearlyChartShow ?? false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        setDidMount(true);
        prepareActiveUsers();
    }, []);

    const renderEntitiesName = () => {
        let entitiesName = [];
        if (!entityNames) return entitiesName;
        
        for(let i=0;i<entityNames.length;i++)
        {
            entitiesName.push(<p key={i}>{entityNames[i]}</p>);
        }
        return entitiesName;
    };

    const renderProfilesActivesChart = () => {
        let dataProfileUsageChart = [{
            value: profilesActive,
            color:"#42f58d",
            highlight: "#97fcc2",
            label: LanguageManager.getTranslation("school-module.profiles-active", languages)
        }, {
            value:  profilesNonActive,
            color: "#fa697c",
            highlight: "#ff425b",
            label: LanguageManager.getTranslation("school-module.profiles-non-active", languages)
        }];

        return  <PieChart chartData={dataProfileUsageChart} widthChart={"100"} heightChart={"100"}/>
    };

    const dynamicSort = (property) => {
        let sortOrder = 1;
        if(property[0] === "-") {
            sortOrder = -1;
            property = property.substr(1);
        }
        return function (a,b) {
            /* next line works with strings and numbers,
             * and you may want to customize it to your needs
             */
            let result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
            return result * sortOrder;
        }
    };

    const renderUsageDatesReportMonths = () => {
        if(typeof monthDetails === 'undefined' || monthDetails === null || monthDetails.length >= 32)
            return null;

        // Prepare data
        let dataSet = [];
        let legend = [];

        let arrayMonths = monthDetails.sort(dynamicSort("Month"));

        for(let i=0; i < arrayMonths.length;i++)
        {
            arrayMonths[i].Month = arrayMonths[i].Month.substring(0, 7);
            const r = Math.floor(Math.random() * 200);
            const g = Math.floor(Math.random() * 200);
            const b = Math.floor(Math.random() * 200);
            let color = 'rgb(' + r + ',' + g + ',' + b + ')';

            legend.push(<div key={i}><p>{arrayMonths[i].Month}{' Total: '}{arrayMonths[i].MonthTotal}
                <LegendCanvas color={color}/></p></div>);
            let newDateDetail = {
                label: arrayMonths[i].Month,
                fillColor: color,
                strokeColor: color,
                pointColor: color,
                pointStrokeColor: '#fff',
                pointHighlightFill: '#fff',
                pointHighlightStroke: color,
                data: [arrayMonths[i].MonthTotal],
            };
            dataSet.push(newDateDetail);
        }

        let chartLineData = {
            labels: [LanguageManager.getTranslation("school-module.usage-per-month", languages)],
            datasets:dataSet
        };

        const options = {
            scaleShowGridLines: true,
            scaleGridLineColor: 'rgba(0,0,0,.05)',
            scaleGridLineWidth: 1,
            scaleShowHorizontalLines: true,
            scaleShowVerticalLines: true,
            bezierCurve: true,
            bezierCurveTension: 0.4,
            pointDot: true,
            pointDotRadius: 4,
            pointDotStrokeWidth: 1,
            pointHitDetectionRadius: 20,
            datasetStroke: true,
            datasetStrokeWidth: 2,
            datasetFill: true,
            legendTemplate: '<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].strokeColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',
        };

        return <div>
                <Row>
                    <Col xs={12}>
                        <b><Translate id="school-module.usage-per-month"/></b>
                    </Col>
                </Row>
            <Row>
            <Col lg={{size: 8}} md={{size: 8}} sm={{size: 12}} xs={{size: 12}}>
                <ChartBar data={chartLineData}
                          options={options}
                          width="600" height="250"/>
            </Col>
            <Col lg={{size: 4}} md={{size: 4}} sm={{size: 12}} xs={{size: 12}}>
                {legend}
            </Col>
        </Row>
        </div>
    };

    const renderUsageDatesReport = () => {
        if(typeof dateDetails === 'undefined' || dateDetails === null || dateDetails.length >= 32)
            return null;

        // Prepare data
        let dataSet = [];
        let legend = [];

        let arrayDates = dateDetails.sort(dynamicSort("FullDate"));
        for(let i=0; i < arrayDates.length;i++)
        {
            arrayDates[i].FullDate = arrayDates[i].FullDate.substring(0, 10);
            const r = Math.floor(Math.random() * 200);
            const g = Math.floor(Math.random() * 200);
            const b = Math.floor(Math.random() * 200);
            let color = 'rgb(' + r + ',' + g + ',' + b + ')';
            legend.push(<div key={i}><p>{arrayDates[i].FullDate}{' Total: '}{arrayDates[i].DateTotal}
                <LegendCanvas color={color}/></p></div>);
            let newDateDetail = {
                label: arrayDates[i].FullDate,
                fillColor: color,
                strokeColor: color,
                pointColor: color,
                pointStrokeColor: '#fff',
                pointHighlightFill: '#fff',
                pointHighlightStroke: color,
                data: [arrayDates[i].DateTotal],
            };
            dataSet.push(newDateDetail);
        }

        let chartLineData = {
            labels: [LanguageManager.getTranslation("school-module.usage-per-day", languages)],
            datasets:dataSet
        };

        const options = {
            scaleShowGridLines: true,
            scaleGridLineColor: 'rgba(0,0,0,.05)',
            scaleGridLineWidth: 1,
            scaleShowHorizontalLines: true,
            scaleShowVerticalLines: true,
            bezierCurve: true,
            bezierCurveTension: 0.4,
            pointDot: true,
            pointDotRadius: 4,
            pointDotStrokeWidth: 1,
            pointHitDetectionRadius: 20,
            datasetStroke: true,
            datasetStrokeWidth: 2,
            datasetFill: true,
            legendTemplate: '<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].strokeColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',
        };

        return <div>
            <Row>
                <Col xs={12}>
                    <b><Translate id="school-module.usage-per-day"/></b>
                </Col>
            </Row>
            <Row>
            <Col lg={{size: 8}} md={{size: 8}} sm={{size: 12}} xs={{size: 12}}>
            <ChartBar data={chartLineData}
                          options={options}
                          width="600" height="250"/>
                </Col>
            <Col lg={{size: 4}} md={{size: 4}} sm={{size: 12}} xs={{size: 12}}>
            {legend}
               </Col>
            </Row>
        </div>
    };

    const renderUsageDatesReportYears = () => {
        if(typeof yearDetails === 'undefined' || yearDetails === null || yearDetails.length <= 1)
            return null;

        // Prepare data
        let dataSet = [];
        let legend = [];

        let arrayYears = yearDetails.sort(dynamicSort("Year"));

        for(let i=0; i < arrayYears.length;i++)
        {
            arrayYears[i].Year = arrayYears[i].Year.substring(0, 4);
            const r = Math.floor(Math.random() * 200);
            const g = Math.floor(Math.random() * 200);
            const b = Math.floor(Math.random() * 200);
            let color = 'rgb(' + r + ',' + g + ',' + b + ')';

            legend.push(<div key={i}><p>{arrayYears[i].Year}{' Total: '}{arrayYears[i].YearTotal}
                <LegendCanvas color={color}/></p></div>);
            let newDateDetail = {
                label: arrayYears[i].Year,
                fillColor: color,
                strokeColor: color,
                pointColor: color,
                pointStrokeColor: '#fff',
                pointHighlightFill: '#fff',
                pointHighlightStroke: color,
                data: [arrayYears[i].YearTotal],
            };
            dataSet.push(newDateDetail);
        }

        let chartLineData = {
            labels: [LanguageManager.getTranslation("school-module.usage-per-year", languages)],
            datasets:dataSet
        };

        const options = {
            scaleShowGridLines: true,
            scaleGridLineColor: 'rgba(0,0,0,.05)',
            scaleGridLineWidth: 1,
            scaleShowHorizontalLines: true,
            scaleShowVerticalLines: true,
            bezierCurve: true,
            bezierCurveTension: 0.4,
            pointDot: true,
            pointDotRadius: 4,
            pointDotStrokeWidth: 1,
            pointHitDetectionRadius: 20,
            datasetStroke: true,
            datasetStrokeWidth: 2,
            datasetFill: true,
            legendTemplate: '<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].strokeColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',
        };


        return <div>
            <Row>
                <Col xs={12}>
                    <b><Translate id="school-module.usage-per-year"/></b>
                </Col>
            </Row>
        <Row>
            <Col lg={{size: 8}} md={{size: 8}} sm={{size: 12}} xs={{size: 12}}>
                <ChartBar data={chartLineData}
                          options={options}
                          width="600" height="250"/>
            </Col>
            <Col lg={{size: 4}} md={{size: 4}} sm={{size: 12}} xs={{size: 12}}>
                {legend}
            </Col>
        </Row>
            </div>
    };

    const prepareActiveUsers = () => {
        if(typeof  profilesDetails === 'undefined' ||
            profilesDetails === null ||
            profilesDetails.length === 0)
            return null;

        let arrayUsers = profilesDetails.sort(dynamicSort("GivenName"));

        let trDetailsActive = [];
        let trDetailsNonActive = [];
        for(let i=0; i < arrayUsers.length;i++) {
            if (arrayUsers[i].Active) {
                    trDetailsActive.push(<tr key={i}>
                        <td className="tr-1">{arrayUsers[i].UserDetails.UserFullNameModel.GivenName}{' '}
                            {arrayUsers[i].UserDetails.UserFullNameModel.Surname}
                        </td>
                        <td className="tr-1">{arrayUsers[i].UserDetails.UserAccountModel.Username}</td>
                    </tr>);
            }
            else
            {
                    trDetailsNonActive.push(<tr key={i}>
                        <td className="tr-2">{arrayUsers[i].UserDetails.UserFullNameModel.GivenName}{' '}
                            {arrayUsers[i].UserDetails.UserFullNameModel.Surname}
                        </td>
                        <td className="tr-2">{arrayUsers[i].UserDetails.UserAccountModel.Username}</td>
                    </tr>);

            }
        }

        setActiveUsersTRs(trDetailsActive);
        setInactiveUsersTRs(trDetailsNonActive);
    };

    const renderNonActiveUsers = () => {
       return <Row>
            <Col xs={12}>
                <b><Translate id="school-module.non-active-users"/></b>
            </Col>
            <Col xs={12}>
                <Table>
                    <tbody>
                        {inactiveUsersTRs}
                    </tbody>
                </Table>
            </Col>
        </Row>;
    };

    const renderActiveUsers = () => {
        return <Row>
            <Col xs={12}>
                <b><Translate id="school-module.active-users"/></b>
            </Col>
            <Col xs={12}>
                <Table>
                    <tbody>
                        {activeUsersTRs}
                    </tbody>
                </Table>
            </Col>
        </Row>;
    };

    const renderSubscriptions = () => {
        if (!subscriptionsDetails) return null;
        
        let trSubscriptions = [];
        for(let i=0; i < subscriptionsDetails.length;i++) {
            trSubscriptions.push(<tr key={i}>
                    <td className="tr-1">{subscriptionsDetails[i].Plan.Name}</td>
                <td className="tr-1">{subscriptionsDetails[i].Subscription.Status}</td>
                </tr>);
        }

        return <Row>
            <Col xs={12}>
                <b><Translate id="school-module.active-users"/></b>
            </Col>
            <Col xs={12}>
                <Table>
                    <tbody>
                        {trSubscriptions}
                    </tbody>
                </Table>
            </Col>
        </Row>;
    };

    // Renders the main page
    return (
        <div className="print-wrapper">
            <div className="padding-20 card-white">
                <Row>
                    <Col lg={{size: 8}} md={{size:8}} sm={{size: 8}} xs={{size: 8}}>
                        <h2> <Translate id="school-module.school-reports"/></h2>
                    </Col>
                    <Col lg={{size: 4}} md={{size:4}} sm={{size: 4}} xs={{size: 4}}>
                        <Media src={logo} alt="Company logo"
                               className="logo-mob"/>
                        </Col>
                    </Row>
                    <hr/>
                    <Row>
                        <Col xs={12}>
                            <b><Translate id="school-module.entities-applied"/></b>
                            <br/>
                            {renderEntitiesName()}
                        </Col>
                    </Row>
                    <Row>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            <Translate id="school-module.date-from"/> {' '}
                            {dateFrom}
                        </Col>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            <Translate id="school-module.date-to"/> {' '}
                            {dateTo}
                        </Col>
                    </Row>
                    <hr/>
                    <Row>
                        <Col lg={{size: 4}} md={{size: 4}} sm={{size: 12}} xs={{size: 12}}>
                            <h3><Translate id="school-module.profiles-info"/></h3>
                                <b><Translate id="school-module.profiles-quantity"/>{': '}</b>
                                {profilesQuantity}
                                <br/>
                                <b><Translate id="school-module.profiles-active"/>{': '}</b>
                                {profilesActive}
                                <br/>
                            <b><Translate id="school-module.profiles-non-active"/>{': '}</b>
                            {profilesNonActive}
                        </Col>
                        <Col lg={{size: 4}} md={{size: 4}} sm={{size: 12}} xs={{size: 12}}>
                            {renderProfilesActivesChart()}
                        </Col>
                    </Row>
                <hr/>
                <Row>
                    <Col xs={12}>
                        <h3><Translate id="school-module.usage-dates"/></h3>
                    </Col>
                </Row>
                {renderUsageDatesReport()}
                {renderUsageDatesReportMonths()}
                {renderUsageDatesReportYears()}
                <hr/>
                {renderSubscriptions()}
                <hr/>
                {renderActiveUsers()}
                <hr/>
                {renderNonActiveUsers()}
            </div>
        </div>
    );
};

export default withLocalize(ChartReportUsage);