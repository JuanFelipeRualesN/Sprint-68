import React, { useState, useMemo, useEffect, useCallback } from 'react';
import {
    Modal, ModalHeader, ModalBody, ModalFooter,
    Button, Input, Pagination, PaginationItem, PaginationLink,
    Spinner
} from 'reactstrap';
import { Translate, withLocalize } from 'react-localize-redux';

// Assuming the API function and the mapping function are now imported or defined here.
import { getReadingQuestionDiagnostics } from '../../../../../api/athenas';
import QuestionItem from '../question-item';

// --- FALLBACK PLACEHOLDER (Used if API fails/empty) ---
const fallbackQuestions = [{
    id: "R001",
    code: "READ01",
    title: "Vocabulary in Context (Fallback)",
    subject: "English Reading",
    grade: "1st",
    standard: "1.R.1.1",
    text: "Reading is a ____ way to learn.",
    answers: [
        { id: "A", text: "wonderful", isCorrect: true },
        { id: "B", text: "bad", isCorrect: false },
    ]
}];

/** Map API -> UI question shape */
function mapApiToUi(apiItems) {
    if (!Array.isArray(apiItems)) return [];

    return apiItems.map((item, idx) => ({
        id: item?.QuestionId || `R${String(idx + 1).padStart(3, '0')}`, // Use a real ID if available
        code: item?.ConstructedFullCode || '',
        title: item?.Question || 'Reading Question',
        subject: item?.SubjectCode || '',
        grade: item?.LevelCode || '',
        standard: item?.ConstructedFullCode || '',
        text: item?.Question || '',
        answers: (item?.Responses || []).map((r, i) => ({
            id: String.fromCharCode(65 + i),
            text: r?.Text ?? '',
            isCorrect: !!r?.IsCorrect
        }))
    }));
}

// ITEMS_PER_PAGE is the 'limit' parameter sent to the backend
const ITEMS_PER_PAGE = 10;

function QuestionSearchModal({
                                                isOpen,
                                                onClose,
                                                onSelectQuestion,
                                                existingQuestionIds,
                                                subjectFilter,
                                                gradeFilter,
    translate
                                            }) {
    // State for Questions and Loading
    const [questions, setQuestions] = useState([]);
    const [totalResults, setTotalResults] = useState(0);
    const [isLoading, setIsLoading] = useState(false);

    // State for search and pagination parameters
    const [searchQuery, setSearchQuery] = useState('');
    const [currentPage, setCurrentPage] = useState(1);

    // **FIXED LOGIC**: Use a state variable to track the filters that caused the last fetch.
    const [filterKey, setFilterKey] = useState(JSON.stringify({ subjectFilter, gradeFilter, searchQuery }));

    // Calculate total pages based on the total results from the backend
    const totalPages = Math.ceil(totalResults / ITEMS_PER_PAGE);

    // --- Data Fetching Function (Includes Pagination/Search Params) ---
    const fetchQuestions = useCallback(async (page, query) => {
        setIsLoading(true);
        let subjectCode = 'adl'; // Default or 'all languages'

        if(subjectFilter === "English") {
            subjectCode = "en";
        } else if(subjectFilter === "Español") {
            subjectCode = "sp";
        }

        try {
            // *** Assume API returns an object { data: Question[], totalCount: number } ***
            const apiResponse = await getReadingQuestionDiagnostics(
                subjectCode,   // subjectCode
                gradeFilter,   // levelCode
                query,         // constructedCodeLike filter
                page,          // fromPage
                ITEMS_PER_PAGE // limit
            );

            // ⭐️ CRUCIAL FIX: Get the true total count from the API response
            const totalCount = apiResponse?.data?.Total || 0;
            const apiData = apiResponse?.data?.Items || [];

            const mapped = mapApiToUi(apiData).filter(q => q && q.id);

            setQuestions(mapped.length > 0 ? mapped : []);
            setTotalResults(totalCount); // Use the correct total count

        } catch (err) {
            console.error('Failed to load diagnostic questions in modal:', err);
            setQuestions(fallbackQuestions);
            setTotalResults(fallbackQuestions.length); // Fallback total
        } finally {
            setIsLoading(false);
        }
        // Dependencies only on filters/search when the function is defined
    }, [subjectFilter, gradeFilter]);


    // --- Effect runs when modal opens, page changes, OR filterKey changes (which forces a page reset/fetch) ---
    useEffect(() => {
        if (!isOpen) return;

        // The only dependency that changes outside of this hook is currentPage.
        // If filterKey changes, it forces a fetch at page 1. If currentPage changes, it forces a fetch.
        fetchQuestions(currentPage, searchQuery);

    }, [isOpen, currentPage, filterKey, fetchQuestions, searchQuery]);
    // Added searchQuery to dependencies just to be explicit for the linter,
    // though filterKey usually covers its change.

    // --- Handlers ---
    const handleSearch = (e) => {
        const newSearchQuery = e.target.value;
        setSearchQuery(newSearchQuery);

        // ⭐️ CORRECT FILTER/SEARCH CHANGE LOGIC:
        // 1. Reset page to 1
        setCurrentPage(1);
        // 2. Update the filter key to force a re-fetch (even if currentPage was already 1)
        setFilterKey(JSON.stringify({ subjectFilter, gradeFilter, searchQuery: newSearchQuery }));
    };

    const handlePageChange = (newPage) => {
        if (newPage >= 1 && newPage <= totalPages) {
            setCurrentPage(newPage);
        }
        // Allow forward movement if totalPages is 0 (i.e., total count is not yet known)
        // but the backend provides data, though this is less common with strict server-side.
        if (totalPages === 0 && newPage > currentPage) {
            setCurrentPage(newPage);
        }
    };


    return (
        <Modal isOpen={isOpen} toggle={onClose} size="lg">
            <ModalHeader toggle={onClose}>🔍 <Translate id="dashboards.search_and_select_questions">Search and Select Questions</Translate></ModalHeader>
            <ModalBody>

                <Input
                    type="text"
                    placeholder={translate("dashboards.search-by-code")}
                    value={searchQuery}
                    onChange={handleSearch}
                    className="mb-3"
                />

                {/* Show Loading Spinner */}
                {isLoading && (
                    <div className="text-center py-4">
                        <Spinner color="primary" />
                    </div>
                )}

                {/* Show No Results/No Match Message */}
                {!isLoading && questions.length === 0 && (
                    <p className="text-muted">
                        <Translate id="dashboards.no_questions_match">No questions match your search criteria.</Translate>
                    </p>
                )}

                {/* Render Questions (no client-side pagination needed here) */}
                {!isLoading && questions.filter(q => q && q.id)
                    .map(q => (
                        <QuestionItem
                            key={q.id}
                            question={q}
                            onSelect={() => onSelectQuestion(q)}
                            isSelected={existingQuestionIds.includes(q.id)}
                        />
                    ))}
            </ModalBody>
            <ModalFooter className="d-flex justify-content-between">
                <Pagination size="sm" className="m-0">
                    <PaginationItem disabled={currentPage === 1 || isLoading}>
                        <PaginationLink onClick={() => handlePageChange(currentPage - 1)}>
                            {/* ⭐️ FIX 1: Removed 'previous' prop. Added translated text. */}
                            <Translate id="dashboards.previous">Previous</Translate>
                        </PaginationLink>
                    </PaginationItem>

                    {/* Show current page indicator */}
                    <PaginationItem active>
                        <PaginationLink>{currentPage}</PaginationLink>
                    </PaginationItem>

                    {/* Disable NEXT if we've reached the known total pages or during loading */}
                    <PaginationItem disabled={currentPage >= totalPages || totalResults === 0 || isLoading}>
                        <PaginationLink onClick={() => handlePageChange(currentPage + 1)}>
                            {/* ⭐️ FIX 2: Removed 'next' prop. Added translated text. */}
                            {translate("dashboards.next")}
                        </PaginationLink>
                    </PaginationItem>
                </Pagination>

                <Button color="secondary" onClick={onClose}>
                    {translate("dashboards.close")}
                </Button>
            </ModalFooter>
        </Modal>
    );
}

export default withLocalize(QuestionSearchModal);