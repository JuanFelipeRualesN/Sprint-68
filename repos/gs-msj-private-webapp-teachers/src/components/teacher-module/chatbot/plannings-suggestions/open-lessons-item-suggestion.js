import React, { useState, useEffect, useRef } from "react";
import Border from "../../planning/shared/components/border";
import Heading from "../../planning/shared/components/heading";
import FormCheckbox from "../../planning/shared/components/form/form-checkbox";
import FormField from "../../planning/shared/components/form/form-field";
import Text from "../../planning/shared/components/text";
import Alert from "../../planning/shared/components/alert";
import { Translate } from "react-localize-redux";
import { translate } from "../../planning/shared/utils";
import { getSuggestionFromChatbot } from "../../../../api/chatbot";
import "./styles.css";

// Component with suggestions from chatbot API for open lessons
const OpenLessonsItemWithSuggestions = ({name, fieldType, values = [], value = "", handleChange, planningContext, label = null, isSingleField = false}) => {
  const [isActive, setIsActive] = useState(values?.length > 0 || value?.length > 0);
  const [currentInput, setCurrentInput] = useState(value || "");
  const [suggestions, setSuggestions] = useState([]);
  const [showSuggestions, setShowSuggestions] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const debounceTimer = useRef(null);
  const suggestionBoxRef = useRef(null);

  useEffect(() => {
    if (isSingleField) {
      setCurrentInput(value || "");
    }
  }, [value, isSingleField]);

  // Fetch suggestions from backend
  const fetchSuggestions = async (input) => {
    const trimmedInput = input.trim();
    
    if (trimmedInput.length < 2) {
      setSuggestions([]);
      setIsLoading(false);
      return;
    }

    setIsLoading(true);

    try {
      const response = await getSuggestionFromChatbot(fieldType, trimmedInput, planningContext);
      const fetchedSuggestions = response.data.suggestions || [];
      
      setSuggestions(fetchedSuggestions);
    } catch (error) {
      console.error('Error fetching suggestions:', error);
      setSuggestions([]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleInputChange = (newValue) => {
    setCurrentInput(newValue);
    
    if (newValue.trim().length > 0) {
      setShowSuggestions(true);
    } else {
      setShowSuggestions(false);
      setSuggestions([]);
    }
  };

  // Limit API calls
  useEffect(() => {
    if (debounceTimer.current) {
      clearTimeout(debounceTimer.current);
    }

    if (!showSuggestions) {
      return;
    }

    if (!isSingleField && !isActive) {
      return;
    }

    debounceTimer.current = setTimeout(() => {
      fetchSuggestions(currentInput);
    }, 500);

    return () => {
      if (debounceTimer.current) {
        clearTimeout(debounceTimer.current);
      }
    };
  }, [currentInput, isActive, showSuggestions, isSingleField]);

  // Close suggestions when clicking outside
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (suggestionBoxRef.current && !suggestionBoxRef.current.contains(event.target)) {
        setShowSuggestions(false);
      }
    };

    if (showSuggestions) {
      document.addEventListener('mousedown', handleClickOutside);
    }

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [showSuggestions]);

  // Add suggestion to field
  const handleAddSuggestion = (suggestion) => {
    if (isSingleField) {
      handleChange(name, suggestion.text);
      setCurrentInput(suggestion.text);
    } else {
      const currentValues = values || [];
      const newItem = { label: suggestion.text, value: suggestion.text };
      
      const exists = currentValues.some(item => 
        (typeof item === 'string' ? item : item.value) === suggestion.text
      );
      
      if (!exists) {
        handleChange(name, [...currentValues, newItem]);
      }
      setCurrentInput("");
    }
    
    setSuggestions([]);
    setShowSuggestions(false);
  };

  const handleRemove = (index) => {
    const newValues = [...values];
    newValues.splice(index, 1);
    handleChange(name, newValues);
    
    if (newValues.length === 0) {
      setIsActive(false);
    }
  };

  const handleManualAdd = () => {
    const trimmedInput = currentInput.trim();
    
    if (!trimmedInput) return;

    if (isSingleField) {
      handleChange(name, trimmedInput);
    } else {
      const currentValues = values || [];
      const newItem = { label: trimmedInput, value: trimmedInput };
      
      const exists = currentValues.some(item => 
        (typeof item === 'string' ? item : item.value) === trimmedInput
      );
      
      if (!exists) {
        handleChange(name, [...currentValues, newItem]);
      }
      setCurrentInput("");
    }
    
    setSuggestions([]);
    setShowSuggestions(false);
  };

  const getDisplayText = (item) => {
    if (typeof item === 'string') return item;
    return item.label || item.value || item;
  };

  const renderDropdownContent = () => {
    
    if (isLoading) {
      return (
        <div className="suggestions-select-loading">
          <span className="spinner-suggestons"></span>
          <Translate id="chatbot.loading-suggestions" />
        </div>
      );
    }
    
    if (suggestions.length > 0) {
      return suggestions.map((suggestion, idx) => (
        <div
          key={idx}
          className="suggestions-select-option"
          onClick={() => handleAddSuggestion(suggestion)}
        >
          {suggestion.text}
        </div>
      ));
    }
    
    return (
      <div className="suggestions-select-empty">
        <Translate id="chatbot.no-suggestions" />
      </div>
    );
  };

  if (isSingleField) {
    return (
      <div style={{ position: 'relative' }} ref={suggestionBoxRef}>
        <FormField
          name={name}
          label={label}
          value={value}
          onChange={(event) => {
            const newValue = event.target.value;
            handleChange(name, newValue);
            handleInputChange(newValue);
          }}
        />

        {showSuggestions && (
          <div className="suggestions-select-suggestions">
            {renderDropdownContent()}
          </div>
        )}
      </div>
    );
  }

  return (
    <Border side="Bottom">
      <>
        <Heading as="h5">
          <Translate id={`open-lessons.${name}-title`} />
        </Heading>

        <FormCheckbox
          name={`${name}-lesson-item`}
          checked={isActive}
          onChange={setIsActive}
          label={<Translate id="lesson.check-item" />}
          disabled={values?.length > 0}
        />

        {isActive && (
          <>
            <div style={{ position: 'relative' }} ref={suggestionBoxRef}>
              {values && values.length > 0 && (
                <div className="suggestions-select-values" style={{ marginBottom: '8px' }}>
                  {values.map((item, index) => (
                    <div key={index} className="suggestions-select-value">
                      <span>{getDisplayText(item)}</span>
                      <button
                        type="button"
                        className="suggestions-select-remove"
                        onClick={() => handleRemove(index)}
                      >
                        x
                      </button>
                    </div>
                  ))}
                </div>
              )}

              <div>
                <input
                  type="text"
                  className="form-control"
                  value={currentInput}
                  onChange={(e) => handleInputChange(e.target.value)}
                  onKeyPress={(e) => {
                    if (e.key === 'Enter') {
                      e.preventDefault();
                      handleManualAdd();
                    }
                  }}
                  placeholder={translate("planning.enter-the-data")}
                />
              </div>

              {showSuggestions && (
                <div className="suggestions-select-suggestions">
                  {renderDropdownContent()}
                </div>
              )}
            </div>

            <Text type="hint" className="text-black" style={{ marginTop: '8px' }}>
              <>
                <Translate id="accommodation.type-and-press" /> <strong>ENTER</strong>{" "}
                <Translate id="accommodation.to-add" />
              </>
            </Text>
            <Alert
              className="py-0"
              message={<Translate id="accommodation.type-alert" />}
              variant="simple"
              size="sm"
            />
          </>
        )}
      </>
    </Border>
  );
};

export default OpenLessonsItemWithSuggestions;