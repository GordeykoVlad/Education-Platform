CREATE TABLE "user"
(
  "id" BIGSERIAL PRIMARY KEY,
  "login" TEXT NOT NULL,
  "password" TEXT NOT NULL
);
CREATE UNIQUE INDEX user_login_uindex ON "user" ("login");
--Создание уникального логина
