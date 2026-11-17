import { Link } from "react-router-dom";
import { Translate, withLocalize } from "react-localize-redux";
import { isRoleAllowed } from "../../../utils/utils";
import { useState, useMemo, useEffect } from 'react';
import PowerBIEmbedComponent from "../powerbi-embed";
import { reportConfig, componentRegistry } from "../../../reportConfig";
import SessionManager from "../../../custom-classes/session-manager";
import {FaArrowLeft, FaChevronDown, FaChevronRight, FaBars} from "react-icons/all";
import AssistantPanel from "../assistant-panel";
import { ASSISTANT_AVAILABLE, ASSISTANT_ROLES } from "../assistant-panel/config";

import './style.css';
import './sidebar.css';
import './report-view.css';

const Dashboard = ({ activeLanguage, translate }) => {
  const sessionDetails = SessionManager.getSessionDetails();

  const userMenu = useMemo(() => {
    return reportConfig.map(cat => {
      if (cat.allowedRoles && !isRoleAllowed(cat.allowedRoles, sessionDetails.Roles)) {
        return null;
      }

      const validSubItems = (cat.subItems || []).filter(item => {
        if (!Object.hasOwn(item, 'pageName')) return true;
        if (!item.pageName) return false;
        if (!item.allowedRoles) return true;
        return isRoleAllowed(item.allowedRoles, sessionDetails.Roles);
      });

      if (validSubItems.length === 0) {
        return null;
      }

      return { ...cat, subItems: validSubItems };
    }).filter(Boolean);

  }, [sessionDetails]);

  const [activeCategory, setActiveCategory] = useState(null);
  const [activePage, setActivePage] = useState(null);
  const [expandedCatName, setExpandedCatName] = useState(null);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);

  // Asistente de datos junto al informe (REQ-3741): informe de Power BI embebido y estado del panel.
  const [pbiReport, setPbiReport] = useState(null);
  const [assistantOpen, setAssistantOpen] = useState(false);
  // Fase 1: por defecto solo admin y admin-ore (ASSISTANT_ROLES; el servicio aplicará además el alcance por rol del lado del servidor).
  const showAssistant = ASSISTANT_AVAILABLE
    && isRoleAllowed(ASSISTANT_ROLES, sessionDetails.Roles);
  const etiqueta = (clave) => {
    try {
      const texto = translate(clave);
      return typeof texto === "string" && !texto.startsWith("Missing translationId") ? texto : "";
    } catch (_) {
      return "";
    }
  };

  const handleNavClick = (category, page) => {
    setActiveCategory(category);
    setActivePage(page);
    setIsMobileMenuOpen(false);
    setIsMobile(window.innerWidth < 768);
  };

  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth < 768);
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  useEffect(() => {
    if (userMenu.length > 0) {
      const isCurrentValid = userMenu.find(c => c.category === activeCategory?.category);
      
      if (!activeCategory || !isCurrentValid) {
        setActiveCategory(userMenu[0]);
        setActivePage(userMenu[0].subItems[0]);
        setExpandedCatName(userMenu[0].category);
      }
    }
  }, [userMenu, activeCategory]);

  useEffect(() => {
    if (activePage && activePage.type === 'component') setPbiReport(null);   // páginas de descarga: sin informe embebido
  }, [activePage]);

  const renderContent = () => {
    if (!activeCategory || !activePage) return null;

      if (activePage.type === 'component') {
        const SpecificComponent = componentRegistry[activePage.componentKey];
        if (SpecificComponent) {
          return <SpecificComponent />;
        } 
        return <div>Component Not Found: {activePage.componentKey}</div>;
      }

    return (
      <PowerBIEmbedComponent
        workspaceId={activeCategory.workspaceId}
        reportId={activeCategory.reportId}
        pageName={activePage.pageName}
        isMobile={isMobile}
        onReportReady={setPbiReport}
      />
    );
  };

  if (!userMenu.length) return <div className="p-10">Access Denied</div>;

    return (
      <div>
        <div className={`pbi-dashboard-layout${showAssistant && assistantOpen ? ' assistant-open' : ''}`}>
          {isMobileMenuOpen && (
            <div 
              className="dash-mobile-overlay" 
              onClick={() => setIsMobileMenuOpen(false)} 
            />
          )}
      
          <div className={`dash-sidebar ${isMobileMenuOpen ? 'mobile-open' : ''}`}>
            <Link to="/dashboard" className="dash-back-btn">
              <FaArrowLeft className="dash-back-icon" />
              <span><Translate id="data-analysis.back" /></span>
            </Link>
            <div className="dash-sidebar-header">
              <Translate
                  id="navbar.deliverables"
                  options={{ renderInnerHtml: true }}
              />
            </div>
        
            {userMenu.map((cat) => (
            <div key={cat.category} className="dash-nav-group">
            
            <div 
              className={`dash-nav-group-title ${activeCategory?.category === cat.category ? 'active' : ''}`}
              onClick={() => setExpandedCatName(expandedCatName === cat.category ? null : cat.category)}
            >
              <span><Translate id={`data-analysis.${cat.category}`} /></span>
              <span>{expandedCatName === cat.category ? <FaChevronDown size={16} /> : <FaChevronRight size={16} />}</span>
            </div>

            <div className={`dash-sub-menu ${expandedCatName === cat.category ? 'open' : ''}`}>
              {cat.subItems.map((item) => (
                <div 
                  key={item.label}
                  className={`dash-nav-item ${activePage?.label === item.label ? 'active-page' : ''}`}
                  onClick={() => handleNavClick(cat, item)}
                >
                  <Translate id={`data-analysis.${item.label}`} />
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>

          <div className="dash-main-content">
            <div className="dash-mobile-header">
           <button 
             className="dash-hamburger-btn" 
             onClick={() => setIsMobileMenuOpen(true)}
           >
             <FaBars size={20} />
           </button>
           <span className="dash-mobile-title">{<Translate id={`data-analysis.${activePage?.label}`} /> || 'Dashboard'}</span>
        </div>
        <div className="dash-report-container">
            {renderContent()}
        </div>
      </div>
          {showAssistant && (
            <AssistantPanel
              report={pbiReport}
              contextoReporte={etiqueta(`data-analysis.${activeCategory?.category}`)}
              contextoPagina={etiqueta(`data-analysis.${activePage?.label}`)}
              idioma={activeLanguage && activeLanguage.code}
              onToggle={setAssistantOpen}
            />
          )}
        </div>
    </div>
  );
};

export default withLocalize(Dashboard);
