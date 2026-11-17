import React from 'react';
import { Calendar, momentLocalizer } from 'react-big-calendar';
import moment from 'moment';
import dayjs from 'dayjs';
import { Row, Col, Card, CardBody } from 'reactstrap';
import 'react-big-calendar/lib/css/react-big-calendar.css';

const localizer = momentLocalizer(moment);

const statusColors = {
    published: '#4CA7A2',
    draft:     '#9098AC',
    cancelled: '#dc3545',
};

const calendarMessages = {
    today:            'Hoy',
    previous:         'Anterior',
    next:             'Siguiente',
    month:            'Mes',
    week:             'Semana',
    day:              'Día',
    agenda:           'Agenda',
    date:             'Fecha',
    time:             'Hora',
    event:            'Evento',
    noEventsInRange:  'No hay eventos en este período.',
    showMore:         total => `+${total} más`,
};

const EventsCalendar = ({ events, onSelectEvent }) => {
    const calendarEvents = events.map(e => ({
        id:       e.id,
        title:    e.title,
        start:    dayjs(`${e.date}T${e.time}`).toDate(),
        end:      dayjs(`${e.date}T${e.endTime}`).toDate(),
        resource: e,
        color:    statusColors[e.status] || '#197BAA',
    }));

    const eventStyleGetter = (event) => ({
        style: {
            backgroundColor: event.color,
            borderColor:     event.color,
            borderRadius:    '4px',
            color:           '#fff',
            fontSize:        '12px',
            padding:         '2px 6px',
        },
    });

    const handleSelectEvent = (calEvent) => {
        onSelectEvent(calEvent.resource);
    };

    return (
        <Row className="clear-style-card td-responsive mt-3">
            <Col xs={12}>
                <Card>
                    <CardBody>
                        <div style={{ marginBottom: '0.75em', display: 'flex', gap: '1em', fontSize: '12px', color: '#4B4B4B' }}>
                            <span>
                                <span style={{ display: 'inline-block', width: 12, height: 12, borderRadius: 2, backgroundColor: '#4CA7A2', marginRight: 4 }} />
                                Publicado
                            </span>
                            <span>
                                <span style={{ display: 'inline-block', width: 12, height: 12, borderRadius: 2, backgroundColor: '#9098AC', marginRight: 4 }} />
                                Borrador
                            </span>
                            <span>
                                <span style={{ display: 'inline-block', width: 12, height: 12, borderRadius: 2, backgroundColor: '#dc3545', marginRight: 4 }} />
                                Cancelado
                            </span>
                        </div>
                        <Calendar
                            localizer={localizer}
                            events={calendarEvents}
                            startAccessor="start"
                            endAccessor="end"
                            style={{ height: 580 }}
                            eventPropGetter={eventStyleGetter}
                            onSelectEvent={handleSelectEvent}
                            messages={calendarMessages}
                            defaultView="month"
                            popup
                        />
                    </CardBody>
                </Card>
            </Col>
        </Row>
    );
};

export default EventsCalendar;
