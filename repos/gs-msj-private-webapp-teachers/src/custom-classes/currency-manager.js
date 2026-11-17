/**
 * Created by portiz on 11/8/18.
 */
class CurrencyManager {

    /**
     * Saves the currency in the local storage
     * @param code The currency ISO Code
     */
    static setCurrency(code){
        localStorage.setItem('currency', code);
    }

    /**
     * Returns the user selected currency
     * @param defaultCurrency default currency code
     * @returns {*} returns the selected default currency
     */
    static getCurrency(defaultCurrency)
    {
        let currencyCode = localStorage.getItem('currency');
        if(currencyCode != null && currencyCode.length > 0)
            return currencyCode;
        else
            return defaultCurrency;
    }
}

export default CurrencyManager;