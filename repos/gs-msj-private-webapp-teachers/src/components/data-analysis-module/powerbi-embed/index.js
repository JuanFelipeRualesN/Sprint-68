import { useState, useEffect, useRef } from 'react';
import { PowerBIEmbed } from 'powerbi-client-react';
import { models } from 'powerbi-client';
import { getPowerBIReports } from "../../../api/data-warehouse";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";

const PowerBIEmbedComponent = ({ reportId, workspaceId, pageName, isMobile }) => {
    const [embedConfig, setEmbedConfig] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    
    const reportRef = useRef(null);

    useEffect(() => {
        let isMounted = true;
        
        setEmbedConfig(null);
        setIsLoading(true);
        reportRef.current = null;

        const fetchEmbedInfo = async () => {
            if (!reportId || !workspaceId) return;

            try {
                const response = await getPowerBIReports(reportId, workspaceId);
                
                if (response.status !== 200) {
                     return;
                }

                const data = response.data.Data;

                if (isMounted) {
                    setEmbedConfig({
                        type: 'report',
                        tokenType: models.TokenType.Embed,
                        accessToken: data.embedToken,
                        embedUrl: data.embedUrl,
                        id: data.reportId,
                        pageName: pageName, 
                        settings: {
                            panes: {
                                filters: { expanded: false, visible: false },
                                pageNavigation: { visible: false },
                            },
                            background: models.BackgroundType.Transparent,
                            layoutType: isMobile ? models.LayoutType.MobilePortrait : models.LayoutType.Master
                        }
                    });
                }
            } catch (error) {
                console.error("Embed Error:", error);
            } finally {
                if (isMounted) setIsLoading(false);
            }
        };

        fetchEmbedInfo();

        return () => { isMounted = false; };
        
    }, [reportId, workspaceId]); 

    useEffect(() => {
        if (reportRef.current && pageName) {
            console.log(`Switch to: ${pageName}`);
            reportRef.current.setPage(pageName)
                .catch(err => console.error("Page switch failed:", err));
        }
    }, [pageName]);


    if (isLoading) {
        return <div ><LoadingFullWindow/></div>;
    }
    
    if (!embedConfig) {
        return <div>No Data</div>;
    }

    return (
        <div style={{ width: '100%', height: '100%' }}>
        <PowerBIEmbed
            embedConfig={embedConfig}
            cssClassName={"report-embed-wrapper"} 
            eventHandlers={new Map([
                ['loaded', async () => {
                if (reportRef.current) {
                    await reportRef.current.refresh();
                }
                }]
            ])} 
            getEmbeddedComponent={(embeddedReport) => {
                reportRef.current = embeddedReport;
            }}
        />
    </div>
    );
};

export default PowerBIEmbedComponent;