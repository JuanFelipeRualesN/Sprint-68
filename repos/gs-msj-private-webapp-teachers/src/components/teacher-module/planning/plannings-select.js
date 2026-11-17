import React, { useEffect, useState } from "react";
import { Dialog } from "@material-ui/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import useGetPlannings from "./shared/hooks/useGetPlannings";
import Button from "./shared/components/button";
import Table from "./shared/components/table";
import Text from "./shared/components/text";
import { translate } from "./shared/utils";
import { Translate } from "react-localize-redux";
import { faPlusCircle, faTimes } from "@fortawesome/free-solid-svg-icons";
import Heading from "./shared/components/heading";

export default function PlanningsSelect({ onSelect }) {
  const [open, setOpen] = useState(false);
  const {
    plannings,
    isLoading: isPlanningLoading,
    getPlannings,
  } = useGetPlannings();
  const headers = [
    translate("planning.topic"),
    translate("planning.subject"),
  ];
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    getPlannings();
  }, []);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleSelectedItem = async (planning) => {
    setIsLoading(true);
    await onSelect(planning);
    setIsLoading(false);
    handleClose();
  };

  const Row = ({ planning }) => {
    return (
      <tr>
        <td data-th={headers[0]}>{planning.name}</td>
        <td data-th={headers[1]}>{planning.subjectAndGrade}</td>
        <td className="table-ppal--td-actions" data-th={""}>
          <Button
            variant="secondary"
            onClick={async () => handleSelectedItem(planning)}
            disabled={isLoading}
          >
            <Translate id="general.select" />
          </Button>
        </td>
      </tr>
    );
  };

  return (
    <>
      <Button
        variant="simple"
        onClick={handleOpen}
        icon={<FontAwesomeIcon icon={faPlusCircle} />}
      >
        <Translate id="planning.add-exising-planning" />
      </Button>
      <Dialog className="overflow-hidden" open={open} onClose={handleClose}>
        <div className="contentModal--confirmation">
          <div className="Modal--confirmation__title mb-4 d-flex justify-content-between align-items-center">
            <Heading as="h2" className="mb-0">
              <Translate id="planning.exising-planning-title" />
            </Heading>
            <div className="contentModal--close" onClick={handleClose}>
              <FontAwesomeIcon icon={faTimes} />
            </div>
          </div>

          <div className="Modal--confirmation-body text-left px-0 pt-4 overflow-hidden">
            <Text type="regular" className="my-4 text-left">
              <Translate id="planning.exising-planning-text" />
            </Text>

            <Table
              isLoading={isPlanningLoading}
              content={plannings}
              headers={headers}
              noContentMessage={<Translate id="planning.plans-empty" />}
              renderRow={(planning, index) => {
                return <Row key={`tr-${index}`} planning={planning} />;
              }}
            />
          </div>

          <div className="Modal--confirmation-footer px-0">
            <div className="Modal--confirmation-footer__colRight w-100 d-flex justify-content-end">
              <Button variant="regular" onClick={handleClose}>
                <Translate id="confirm-dialog.button-cancel" />
              </Button>
            </div>
          </div>
        </div>
      </Dialog>
    </>
  );
}
