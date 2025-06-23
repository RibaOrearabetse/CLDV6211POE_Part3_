-- Create the database
CREATE DATABASE EventEaseDB2;
GO
USE EventEaseDB2;
GO

-- Create the Venue table
CREATE TABLE Venue (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName VARCHAR(255) NOT NULL,
    Location VARCHAR(255) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl VARCHAR(500)
);
GO

-- Create the EventType table
CREATE TABLE EventType (
	EventTypeID INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(100) NOT NULL
);
GO

-- Create the Event table
CREATE TABLE Event (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName VARCHAR(255) NOT NULL,
    EventDate DATETIME NOT NULL,
    Description TEXT,
    VenueId INT NULL,
    EventTypeID INT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venue(VenueId) ON DELETE SET NULL,
    FOREIGN KEY (EventTypeID) REFERENCES EventType(EventTypeID) ON DELETE SET NULL
);
GO

-- Create the Booking table
CREATE TABLE Booking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (EventId) REFERENCES Event(EventId) ON DELETE CASCADE
);
GO

-- Insert sample data into Venue table
INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES 
('Grand Hall', '123 Main St, Johannesburg', 500, 'https://via.placeholder.com/150'),
('Conference Center', '45 Business Rd, Cape Town', 300, 'https://via.placeholder.com/150');
GO

-- Insert sample data into EventType table
INSERT INTO EventType (Name)
VALUES
('Conference'),
('Wedding'),
('Naming'),
('Birthday'),
('Concert');
GO

-- Insert sample data into Event table
INSERT INTO Event (EventName, EventDate, Description, VenueId, EventTypeID)
VALUES 
('Tech Expo 2025', '2025-06-15 09:00:00', 'A large tech industry expo.', 1, 1),
('Business Summit', '2025-07-20 10:00:00', 'Annual business networking event.', 2, 2),
('Community Gathering', '2025-08-01 18:00:00', 'Local community meeting.', 3, 3);
GO

-- Insert sample data into Booking table
INSERT INTO Booking (EventId)
VALUES 
(1),
(2);
GO

DELETE FROM Booking;
DELETE FROM Event;
DELETE FROM EventType;

DBCC CHECKIDENT ('Event', RESEED, 0);
DBCC CHECKIDENT ('Booking', RESEED, 0);
DBCC CHECKIDENT ('EventType', RESEED, 0);



-- Verify inserted data
SELECT * FROM Venue;
SELECT * FROM Event;
SELECT * FROM Booking;
SELECT * FROM EventType;
GO
