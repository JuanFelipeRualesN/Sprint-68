import { useState, useEffect } from 'react';
import "./style.css";
import { Translate } from "react-localize-redux";

const DocumentViewer = ({ blobUrl }) => {
  const [viewerUrl, setViewerUrl] = useState('');

    useEffect(() => {
        if (blobUrl) {
        const encodedUrl = encodeURIComponent(blobUrl);
        const msViewerEndpoint = `https://view.officeapps.live.com/op/embed.aspx?src=${encodedUrl}`;
        
        setViewerUrl(msViewerEndpoint);
        }
  }, [blobUrl]);

  return (
    <div className="document-preview-container">        
        {viewerUrl ? (
        <iframe
            src={viewerUrl}
            width="100%"
            height="100%"
            title="Document Preview"
        ></iframe>
        ) : (
        <div className="document-message">
            <Translate id="data-analysis.excel-error" />
        </div>
        )}
    </div>
    );
};

export default DocumentViewer;