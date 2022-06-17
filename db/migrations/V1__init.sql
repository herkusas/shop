CREATE TABLE IF NOT EXISTS clients
(
    "id"   integer NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "name" character varying(200) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "primary_key_clients" PRIMARY KEY ("id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "UI_clients_name"
    ON clients USING btree
        ("name" COLLATE pg_catalog."default" ASC NULLS LAST);

CREATE TABLE IF NOT EXISTS products
(
    "id"    integer NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "name"  character varying(200) COLLATE pg_catalog."default" NOT NULL,
    "price" decimal NOT NULL,
    CONSTRAINT "primary_key_products" PRIMARY KEY ("id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "UI_products_name"
    ON products USING btree
        ("name" COLLATE pg_catalog."default" ASC NULLS LAST);

CREATE TABLE IF NOT EXISTS orders
(
    "id"        integer NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "client_id" integer NOT NULL,
    CONSTRAINT "primary_key_orders" PRIMARY KEY ("id"),
    CONSTRAINT "foreign_key_client_id" FOREIGN KEY ("client_id")
        REFERENCES clients ("id") MATCH SIMPLE
);

CREATE TABLE IF NOT EXISTS order_products
(
    "id"         integer NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "order_id"   integer NOT NULL,
    "product_id" integer NOT NULL,
    CONSTRAINT "primary_key_order_products" PRIMARY KEY ("id"),
    CONSTRAINT "foreign_key_order_products_order_id" FOREIGN KEY ("order_id")
        REFERENCES orders ("id") MATCH SIMPLE,
    CONSTRAINT "foreign_key_order_products_product_id" FOREIGN KEY ("product_id")
        REFERENCES products ("id") MATCH SIMPLE
);