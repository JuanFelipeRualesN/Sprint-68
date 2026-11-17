import React, {useEffect, useMemo, useState, useRef} from "react";
import { useHistory, Link } from "react-router-dom";
import useCreatePlanning from "./shared/hooks/useCreatePlanning";
import useGetPlanningDetails from "./shared/hooks/useGetPlanningDetails";
import Button from "./shared/components/button";
import Heading from "./shared/components/heading";
import Text from "./shared/components/text";
import Border from "./shared/components/border";
import Alert from "./shared/components/alert";
import {
  Alert as AlertBootstrap,
  ListGroup,
  ListGroupItem,
  Modal,
  ModalBody,
  ModalFooter,
  ModalHeader
} from "reactstrap";
import SaveButton from "./shared/components/save-button";
import {faArrowLeft, faExclamationCircle, faExclamationTriangle, faTimes} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import PlanningsSelect from "./plannings-select";
import PlanningsForm from "./plannings-form";
import { Translate, withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import axios from "../../../api/interceptors";
import {Dialog} from "@material-ui/core";
import Loading from "./shared/components/loading";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";

const PlanningsCreate = () => {
  const navigate = useHistory();
  const [createPlanningModel, setCreatePlanningModel] = useState({});
  const [modalOpen, setModalOpen] = useState(false);
  const [expiredDateResults, setExpiredDateResults] = useState([]);
  const latestResponseRef = useRef(null); // To store response for later use

  const {
    succeeded,
    planningId,
    isLoading = false,
    createPlanning,
      setSucceeded,
  } = useCreatePlanning();

  const { planning, getPlanning } = useGetPlanningDetails();
  const [disabled, setDisabled] = useState(false);

  // Initialize translations based on the current version
  useVersionedTranslations();

  const createNewPlanningHandle = async () => {
    const { HasInvalidDates, ...planningPayload } = createPlanningModel;
    const response = await createPlanning(planningPayload);
    latestResponseRef.current = response;

    if (response && response.status === 201) {
      if (response.data.ExpiredDateResults && response.data.ExpiredDateResults.length > 0) {
        setExpiredDateResults(response.data.ExpiredDateResults);
        setModalOpen(true); // Wait for user confirmation
      } else {
        setSucceeded(true); // Navigate directly
      }
    }
  };

  const handleModalConfirm = () => {
    setModalOpen(false);
    setSucceeded(true); // Now allow navigation
  };

  const goBack = () => {
    navigate.push("/v2/plannings");
  };

  useEffect(() => {
    if (succeeded && latestResponseRef.current) {
      const { Name, OpenDate, CloseDate, PeriodCloseDate, SubjectCode, LevelCode, SubjectName } = createPlanningModel;

      SessionManager.setSelectedPlanning(
          planningId,
          Name,
          false,
          OpenDate,
          CloseDate,
          PeriodCloseDate,
          SubjectName,
          SubjectCode,
          LevelCode
      );

      navigate.push(`/v2/plannings-detail?page=lessons`);
    }
  }, [succeeded]);

  useEffect(() => {
    const fetchActiveSubscriptions = async () => {
      try {
        const endpoint = Settings.getSubscritionsAssociatedTeacherAPI();
        const response = await axios.get(endpoint);
        const data = response.status === 200 ? response.data : [];
        const now = new Date();

        const periods = data.flatMap(item =>
            item.EntityAcademicDetailFullResponseModel?.AcademicPeriodModelList?.filter(p => {
              const to = new Date(p.PeriodTo);
              return to >= now;
            }) || []
        );

        if (data.length === 0 || periods.length === 0) {
          setDisabled(true);
        } else {
          setDisabled(false);
        }
      } catch (error) {
        console.error("Failed to fetch active subscription:", error);
      }
    };

    fetchActiveSubscriptions();
  }, []);



  const isSaveDisabled = useMemo(() => {
    if (createPlanningModel?.HasInvalidDates) return true;

    if (createPlanningModel?.OpenDate && createPlanningModel?.CloseDate) {
      const openDate = new Date(createPlanningModel.OpenDate);
      const closeDate = new Date(createPlanningModel.CloseDate);
      if (openDate > closeDate) return true;
    }

    return (
        !createPlanningModel?.PeriodId ||
        !createPlanningModel?.Name ||
        !createPlanningModel?.SubjectCode ||
        !createPlanningModel?.LevelCode ||
        !createPlanningModel?.OpenDate ||
        !createPlanningModel?.CloseDate ||
        !(createPlanningModel?.WeekNumber > 0)
    );
  }, [createPlanningModel]);

  return (
      <>
        <div className="container-padding animated delay-05s fadeIn">
          <div className="wrapper-principal-container pr-3">
            <div className="wrapper-col wrapper-col--flex-end borderGray-bottom mb-3 block-display-responsive">
              <div className="col col-no-padding-left">
                <Link to="plannings" className="btn-back">
                  <FontAwesomeIcon icon={faArrowLeft} />{" "}
                  <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                </Link>
                <div className="title-principal mt-3">
                  <Translate id="planning.planning-create" options={{ renderInnerHtml: true }} />
                </div>
              </div>

              <div className="section-description mb-3">
                <div className="wrapper-average-progress">
                  <div className="wrapper-average-progress__note">
                    <small>
                      <FontAwesomeIcon icon={faExclamationCircle} className="dt-icons-size" />{" "}
                      <Translate id="planning.planning-closing-notes" options={{ renderInnerHtml: true }} />
                    </small>
                  </div>
                </div>
              </div>

              <AlertBootstrap
                  color="danger"
                  isOpen={disabled}
                  className="alert-info"
              >
                <FontAwesomeIcon icon={faExclamationCircle} className="dt-icons-size" />{" "}
                <span>
                <Translate id="planning.no-active-periods" options={{ renderInnerHtml: true }} />
              </span>
              </AlertBootstrap>

              <Border side="Bottom" className="d-flex">
                <Button
                    variant="regular"
                    onClick={goBack}
                    disabled={isLoading}
                >
                  <Translate id="confirm-dialog.button-cancel" />
                </Button>
                <SaveButton
                    isLoading={isLoading}
                    className="ml-2 btn-secondary"
                    onClick={createNewPlanningHandle}
                    disabled={isSaveDisabled}
                    label={<Translate id="planning.planning-create" />}
                />
              </Border>

              <div className="Region-w-borderBottom">
                <PlanningsSelect onSelect={async (p) => getPlanning(p.id)} />
                <Text type="hint">
                  <Translate id="planning.exising-planning" />
                </Text>
              </div>

              <div>
                <Heading as="h2" className="mt-3">
                  <Translate id="planning.planning-create-new" />
                </Heading>
                <PlanningsForm
                    isForEdit={false}
                    disabled={disabled}
                    planning={planning}
                    onChange={setCreatePlanningModel}
                    isLoading={isLoading}
                />
              </div>
            </div>
          </div>
        </div>

        {/* ✅ Modal for ExpiredDateResults */}
        <Dialog className="overflow-hidden" open={modalOpen}>
          <div className="contentModal--confirmation">
            <div className="contentModal--close" onClick={handleModalConfirm}>
              <FontAwesomeIcon icon={faTimes} />
            </div>
            <div className="region-modalIcon modalIcon__warning">
              <FontAwesomeIcon icon={faExclamationTriangle} />
            </div>
            <div className="Modal--confirmation-body">
              <p className="Modal--confirmation__title">
                <Translate id="planning.caution-message-clone" />
              </p>
              <p className="Modal--confirmation__desc">
                <ListGroup>
                  {expiredDateResults.map((item, index) => (
                      <ListGroupItem key={index}>
                        {item.Section}
                      </ListGroupItem>
                  ))}
                </ListGroup>
              </p>
            </div>
            <div className="Modal--confirmation-footer">
              <div className="Modal--confirmation-footer__colRight w-100 d-flex justify-content-end">
                <Button className="pl btn-base btn-regular  ml-2" onClick={handleModalConfirm}>
                  <Translate id="planning.ok" />
                </Button>
              </div>
            </div>
          </div>
        </Dialog>
      </>
  );
};

export default withLocalize(PlanningsCreate);
