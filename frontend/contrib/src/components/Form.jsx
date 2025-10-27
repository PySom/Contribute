import './Form.css';

const Form = ({
    handleSubmit, 
    userName, 
    onNameChange, 
    onNameBlur,
    nameError,
    isAnonymous, 
    onAnonymousChange, 
    amount, 
    onAmountChange, 
    onAmountBlur, 
    amountError, 
    isNameValid,
    isAmountValid }) => (
    <form className="contrib-form" onSubmit={handleSubmit} noValidate>
            <div>
              <label>
                Name
                <input
                  type="text"
                  value={userName}
                  onChange={onNameChange}
                  onBlur={onNameBlur}
                  disabled={isAnonymous}
                  placeholder={isAnonymous ? 'Giving anonymously' : 'Your name'}
                />
              </label>
            </div>
            {nameError && (
                <div id="name-error" style={{ color: '#ff7b7b', marginTop: 6 }}>
                  {nameError}
                </div>
              )}
            <div>
              <label className="checkbox-label">
                <input
                  type="checkbox"
                  checked={isAnonymous}
                  onChange={(e) => onAnonymousChange(e.target.checked)}
                />{' '}
                Mark as anonymous
              </label>
            </div>

            <div>
              <label>
                Amount (NGN)
                <input
                  type="text"
                  inputMode="decimal"
                  pattern="^(\d|\d*\.\d{1,2})$"
                  value={amount}
                  onChange={onAmountChange}
                  onBlur={onAmountBlur}
                  placeholder="0.00"
                  aria-invalid={!!amountError}
                  aria-describedby={amountError ? 'amount-error' : undefined}
                />
              </label>
              {amountError && (
                <div id="amount-error" style={{ color: '#ff7b7b', marginTop: 6 }}>
                  {amountError}
                </div>
              )}
            </div>

            <div>
              <button type="submit" disabled={!isAmountValid || !isNameValid}>
                Contribute
              </button>
            </div>
          </form>
);

export default Form;