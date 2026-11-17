import { useEffect, useRef, useMemo } from "react";
import { SendPlanAccessLog } from "../utils/access_logger";

/**
* Hook that sends information to tracking when the user is going through the registration and initial payment process.
* User information and the selected offer are sent the first time all fields are valid.
* The information is sent again if there are changes in the plan or the number of students.
*/

function usePaymentCheckoutTracking({
    planId,
    planName,
    studentsQuantity,
    planInterval,
    validFirstName,
    validLastName,
    validEmail,
    emailAvailable,
    validCountry,
    validState,
    validCalculation,
    firstName,
    lastName,
    email,
    country,
    state,
}) {
    const hasSentInitialRef = useRef(false);
    const lastOfferSigRef = useRef(null);
    const debounceRef = useRef(null);

    const offerSig = useMemo(
        () => `${planId}|${studentsQuantity}|${planInterval ?? ""}|${country}`,
        [planId, studentsQuantity, planInterval, country]
    );

    // "Ready to track" = everything valid except card
    const ready = useMemo(
        () =>
            Boolean(
                planId &&
                studentsQuantity > 0 &&
                validFirstName &&
                validLastName &&
                validEmail &&
                emailAvailable === true &&
                validCountry &&
                validState &&
                validCalculation
            ),
        [
            planId,
            studentsQuantity,
            validFirstName,
            validLastName,
            validEmail,
            emailAvailable,
            validCountry,
            validState,
            validCalculation,
        ]
    );

    // not send the tracking if the user no complete this fields
    const identitySig = useMemo(
        () => `${firstName}|${lastName}|${email}|${country}|${state}`,
        [firstName, lastName, email, country, state]
    );


    useEffect(() => {
        clearTimeout(debounceRef.current);
        if (!ready) return;

        const payload = {
            AreaId: "25",
            PlanSubscriptionId: planId,
            PlanSubscriptionName: planName,
            QuantityEnrollment: studentsQuantity,
            Details: "User is registering and paying for a plan",
            ClientInformation: {
                GiveName: firstName,
                SurName: lastName,
                Email: email,
                Country: country,
                State: state,
            },
        };

        // first time all fields are valid
        if (!hasSentInitialRef.current) {
            const currentIdentity = identitySig;
            const currentOffer = offerSig;

            debounceRef.current = setTimeout(() => {

                if (!hasSentInitialRef.current && ready &&
                    identitySig === currentIdentity && offerSig === currentOffer) {
                    try {
                        SendPlanAccessLog(payload);
                    } catch {
                        // empty catch block to avoid breaking the application
                    }
                    hasSentInitialRef.current = true;
                    lastOfferSigRef.current = offerSig;
                }
            }, 1200);

            return () => clearTimeout(debounceRef.current);
        }

        // after the first send, only when plan and number of students change
        if (offerSig !== lastOfferSigRef.current) {
            try {
                SendPlanAccessLog(payload)
            }
            catch (error) {
                // empty catch block to avoid breaking the application
            }
            lastOfferSigRef.current = offerSig;
        }
    }, [ready, offerSig]);
}

export default usePaymentCheckoutTracking;