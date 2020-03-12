#!/bin/bash

host="$1"

shift
password="$@"

echo "Adding the database"

dbCmd() { 
    echo "Connecting to $host with $password"
    false
    while [[ $? -ne 0 ]]
    do
        echo "db connection attempt..."
        dbExists=`/opt/mssql-tools/bin/sqlcmd -S ${host} -U SA -P "${password}" -Q 'IF DB_ID("DevelopmentDB") IS NULL CREATE DATABASE DevelopmentDB;'`
    done
    echo 'ok'
}

dbCmd 
true
