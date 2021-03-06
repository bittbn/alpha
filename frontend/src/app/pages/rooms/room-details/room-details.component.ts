import { Component, OnInit } from '@angular/core';
import {RoomService} from '../../../services/room.service';
import {AuthService} from '../../../services/auth.service';
import {RoleService} from '../../../services/role.service';
import {ActivatedRoute, Router} from '@angular/router';
import {RoomWithDetailsModel} from '../models/room-with-details';
import {ReservationService} from '../../../services/reservation.service';
import {catchError} from 'rxjs/operators';
import {ExceptionService} from '../../../services/exception.service';
import {format} from 'date-fns';
import {ReservationModel} from '../../reservations/models/reservation-model';

@Component({
  selector: 'app-room-details',
  templateUrl: './room-details.component.html',
  styleUrls: ['./room-details.component.css']
})
export class RoomDetailsComponent implements OnInit {

  private roomId: string;
  private room: RoomWithDetailsModel;
  private reservations: ReservationModel[];

  constructor(
    private roomService: RoomService,
    private authService: AuthService,
    private roleService: RoleService,
    private avRoute: ActivatedRoute,
    private router: Router,
    private reservationService: ReservationService,
    private exceptionService: ExceptionService
  ) {
    this.roomId = this.avRoute.snapshot.params.id;
  }

  get isManager(): boolean {
    return this.roleService.IsManager;
  }

  ngOnInit() {
    this.loadRoomDetails(this.roomId);
  }

  loadRoomDetails(roomId: string) {
    this.roomService.getRoomWithDetailsById(roomId)
      .subscribe(data => {
        this.room = data;
        this.reservations = this.room.reservationModels;
      }, catchError(this.exceptionService.throwError)
    );
  }

  getFormatTime(date: Date): string {
    return format(new Date(date), 'dd/MM/yyyy HH:mm');
  }

  editConfirmation(reservationId: string, confirmation: boolean) {
    return this.reservationService.editConfirmationReservation(reservationId, confirmation)
      .subscribe(() => {
          this.loadRoomDetails(this.roomId);
        },
        catchError(this.exceptionService.throwError)
      );
  }
}
