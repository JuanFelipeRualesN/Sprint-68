import React, { useEffect, useState } from "react";
import Loading from "./loading";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";

const NoContent = ({ message }) => {
  return (
    <div className="regionTable-empty">
      <p>
        <FontAwesomeIcon icon={faSearch} />
      </p>
      <p>{message}</p>
    </div>
  );
};

export default function Table({
  headers,
  content = [],
  isLoading,
  renderRow,
  noContentMessage,
}) {
  const [loading, setLoading] = useState(isLoading);

  useEffect(() => {
    setLoading(isLoading);
  }, [isLoading]);

  const RowWithLoading = (loading) => <Loading type="table" show={loading} />;
  return (
    <>
      <table className="ppalTable">
        <thead>
          <tr>
            {headers.map((header, index) => {
              return <th key={`th-${index}`}>{header}</th>;
            })}
            <th></th>
          </tr>
        </thead>
        {loading ? (
          <></>
        ) : (
          <tbody>{content.map((item, index) => renderRow(item, index))}</tbody>
        )}
      </table>

      {loading ? (
        RowWithLoading(loading)
      ) : content && content.length === 0 ? (
        <NoContent message={noContentMessage} />
      ) : (
        <></>
      )}
    </>
  );
}
