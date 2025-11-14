import { useState } from 'react';

const Counter: React.FC = () => {
  const [count, setCount] = useState(0);
  return (
    <div>
      <h1>Counter</h1>
      <p role="status">Current count: {count}</p>
      <button className="btn" onClick={() => setCount(c => c + 1)}>Click me</button>
    </div>
  );
};
export default Counter;
