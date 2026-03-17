import { ApiError } from './http';
import { getErrorMessage } from './error-message';

describe('getErrorMessage', () => {
  it('returns API error messages directly', () => {
    expect(getErrorMessage(new ApiError('Invalid invitation code.', 401), 'fallback')).toBe(
      'Invalid invitation code.',
    );
  });

  it('returns fallback when the error is unknown', () => {
    expect(getErrorMessage(null, 'fallback')).toBe('fallback');
  });
});
