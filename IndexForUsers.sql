CREATE UNIQUE INDEX idx_unique_email ON users(Email);

CREATE INDEX idx_email_password ON users(Email, Password);