import './Form.css';

import { formatAmountInNaira, formatNameOrAnonymous } from '../utils/formatter';

const Contributors = ({ contributions }) => (
    <>
    <h2 id="recent-heading">Recent Contributions</h2>
          {contributions.length === 0 ? (
            <p key="no-contributors">No contributions yet.</p>
          ) : (
            <ul key="contributor-list">
              {contributions.map((c) => (
                <li key={c.name}>
                  <strong>{formatNameOrAnonymous(c.name, c.isAnonymous)}</strong> — {formatAmountInNaira(c.amount)}{' '}
                </li>
              ))}
            </ul>
          )}
    </>
);

export default Contributors;